namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    public partial class AddPlayerController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IPlayerUploadService playersUploadService;

        public AddPlayerController(IPlayerRecordsRepository playerRecordsRepository, IPlayerUploadService playersUploadService)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.playersUploadService = playersUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new AddPlayerModel { CanOverwrite = false };
                return View(model);
            }
            return RedirectToAction(MVC.Account.LogIn());
        }

        [HttpPost]
        public virtual ActionResult Index(AddPlayerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var newPlayer = playersUploadService.GetBattleshipsPlayerFromHttpPostedFileBase(model.File);
                var userWithGivenBotName = playerRecordsRepository.UserWithGivenBotName(newPlayer.Name);

                if (userWithGivenBotName == User.Identity.Name)
                {
                    OverwriteBotFileInitialise(newPlayer.Name, model, model.File);
                    return View(model);
                }

                if (userWithGivenBotName != "")
                {
                    ModelState.AddModelError("", "Battleships player name '" + newPlayer.Name + "' is already taken.");
                    return View(model);
                }

                var playerRecord = playersUploadService.UploadAndGetPlayerRecord(User.Identity.Name, model.File, GetUploadDirectoryPath());
                playerRecordsRepository.Add(playerRecord);
                playerRecordsRepository.SaveContext();
            }
            catch
            {
                ModelState.AddModelError("", "The given file is not a valid player file.");
                return View(model);
            }

            return RedirectToAction(MVC.Players.Index());
        }

        public virtual ActionResult OverwriteYes(AddPlayerModel model)
        {
            System.IO.File.Delete(model.RealPath);
            System.IO.File.Move(model.TemporaryPath, model.RealPath);

            return RedirectToAction(MVC.Players.Index());
        }

        public virtual ActionResult OverwriteNo()
        {
            return RedirectToAction(MVC.AddPlayer.Index());
        }

        private void OverwriteBotFileInitialise(string botName, AddPlayerModel model, HttpPostedFileBase playerFile)
        {
            model.CanOverwrite = true;
            model.TemporaryPath = Path.GetTempFileName();
            model.RealPath = playersUploadService.GenerateFullPath(playerFile, GetUploadDirectoryPath());
            model.FileName = botName + ".dll";
            model.BotName = botName;
            System.IO.File.Delete(model.TemporaryPath);
            playerFile.SaveAs(model.TemporaryPath);
        }

        private string GetUploadDirectoryPath()
        {
            return Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]);
        }
    }
}