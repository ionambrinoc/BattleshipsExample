namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
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
                return View();
            }
            return RedirectToAction(MVC.Account.LogIn());
        }

        [HttpPost]
        public virtual ActionResult Index(FormCollection form)
        {
            var playerFile = Request.Files["file"];
            if (playerFile != null)
            {
                var uploadDirectoryPath = Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]);
                var botFileName = playersUploadService.GetFileName(User.Identity.Name, playerFile);
                TempData["botFileName"] = botFileName;
                if (playerRecordsRepository.GivenFileNameExists(botFileName))
                {
                    playerFile.SaveAs(Path.Combine(uploadDirectoryPath, " temp" + botFileName));
                    return RedirectToAction(MVC.AddPlayer.OverwriteBotFile());
                }
                try
                {
                    var newPlayer = playersUploadService.UploadAndGetPlayerRecord(
                        User.Identity.Name,
                        playerFile,
                        uploadDirectoryPath);

                    if (playerRecordsRepository.GivenBotNameExists(newPlayer.Name))
                    {
                        ModelState.AddModelError("", "Battleships player name '" + newPlayer.Name + "' is already taken.");
                        return View();
                    }

                    playerRecordsRepository.Add(newPlayer);
                    playerRecordsRepository.SaveContext();
                }
                catch
                {
                    ModelState.AddModelError("", "The given file is not a valid player file.");
                    return View();
                }
            }

            return RedirectToAction(MVC.Players.Index());
        }

        public virtual ActionResult OverwriteBotFile()
        {
            TempData["botFileName"] = TempData["botFileName"]; // We need this for the next request if it is OverwriteYes
            return View();
        }

        public virtual ActionResult OverwriteYes()
        {
            try
            {
                var uploadDirectoryPath = Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]);
                var destinationFileName = Path.GetFileName(TempData["botFileName"].ToString()) ?? "";
                var destinationPath = Path.Combine(uploadDirectoryPath, destinationFileName);
                var sourceFileName = " temp" + destinationFileName;
                var sourcePath = Path.Combine(uploadDirectoryPath, sourceFileName);
                System.IO.File.Delete(destinationPath);
                System.IO.File.Move(sourcePath, destinationPath);

                var battleshipsPlayer = playersUploadService.GetBattleshipsPlayerFromFile(destinationFileName);
                playerRecordsRepository.UpdatePlayerRecord(destinationFileName, battleshipsPlayer.Name);
            }
            catch
            {
                TempData["ValidationMessage"] = "The given file is not a valid player file.";
                return RedirectToAction(MVC.AddPlayer.Index());
            }
            return RedirectToAction(MVC.Players.Index());
        }

        public virtual ActionResult OverwriteNo()
        {
            return RedirectToAction(MVC.AddPlayer.Index());
        }
    }
}