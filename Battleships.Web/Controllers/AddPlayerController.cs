namespace Battleships.Web.Controllers
{
    using Battleships.Player;
    using Battleships.Runner.Exceptions;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using System.Configuration;
    using System.IO;
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

            IBattleshipsPlayer newPlayer;

            try
            {
                newPlayer = playersUploadService.LoadBattleshipsPlayerFromFile(model.File);
            }
            catch (InvalidPlayerException)
            {
                ModelState.AddModelError("", "Invalid player file.");
                return View(model);
            }

            if (!playerRecordsRepository.PlayerNameExists(newPlayer.Name))
            {
                var playerRecord = playersUploadService.SaveFileAndGetPlayerRecord(User.Identity.Name, model.File, GetUploadDirectoryPath(), newPlayer.Name);
                playerRecordsRepository.Add(playerRecord);
                playerRecordsRepository.SaveContext();
                return RedirectToAction(MVC.Players.Index());
            }

            if (playerRecordsRepository.PlayerNameExistsForUser(newPlayer.Name, User.Identity.Name))
            {
                InitialiseModelForOverwritingFile(newPlayer.Name, model);
                return View(model);
            }

            ModelState.AddModelError("", "Battleships player name '" + newPlayer.Name + "' is already taken.");
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult OverwriteYes(AddPlayerModel model)
        {
            var realPath = playersUploadService.GenerateFullPath(model.PlayerName, GetUploadDirectoryPath());
            System.IO.File.Delete(realPath);
            System.IO.File.Move(model.TemporaryPath, realPath);

            return RedirectToAction(MVC.Players.Index());
        }

        [HttpPost]
        public virtual ActionResult OverwriteNo()
        {
            return RedirectToAction(MVC.AddPlayer.Index());
        }

        private void InitialiseModelForOverwritingFile(string playerName, AddPlayerModel model)
        {
            model.CanOverwrite = true;
            model.TemporaryPath = Path.GetTempFileName();
            model.PlayerName = playerName;
            System.IO.File.Delete(model.TemporaryPath);
            model.File.SaveAs(model.TemporaryPath);
        }

        private string GetUploadDirectoryPath()
        {
            return Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]);
        }
    }
}
