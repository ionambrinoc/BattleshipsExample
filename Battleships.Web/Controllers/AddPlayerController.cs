namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using Microsoft.AspNet.Identity;
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

            IBattleshipsBot uploadedBot;

            try
            {
                uploadedBot = playersUploadService.LoadBotFromFile(model.File);
            }
            catch (InvalidPlayerException)
            {
                ModelState.AddModelError("", "Invalid player file.");
                return View(model);
            }

            if (!IsValidImage(model.Picture))
            {
                ModelState.AddModelError("", "Invalid image file.");
                return View(model);
            }

            if (!playerRecordsRepository.PlayerNameExists(uploadedBot.Name))
            {
                var playerRecord = playersUploadService.UploadAndGetPlayerRecord(User.Identity.GetUserId(), model.File, model.Picture, uploadedBot.Name);
                playerRecordsRepository.Add(playerRecord);
                playerRecordsRepository.SaveContext();
                return RedirectToAction(MVC.Players.Index());
            }

            if (playerRecordsRepository.PlayerNameExistsForUser(uploadedBot.Name, User.Identity.GetUserId()))
            {
                InitialiseModelForOverwritingFile(uploadedBot.Name, model);
                return View(model);
            }

            ModelState.AddModelError("", "Battleships player name '" + uploadedBot.Name + "' is already taken.");
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult OverwriteYes(AddPlayerModel model)
        {
            playersUploadService.OverwritePlayer(model);

            return RedirectToAction(MVC.Players.Index());
        }

        [HttpPost]
        public virtual ActionResult OverwriteNo()
        {
            return RedirectToAction(MVC.AddPlayer.Index());
        }

        private bool IsValidImage(HttpPostedFileBase picture)
        {
            return picture == null || picture.ContentType.Contains("image");
        }

        private void InitialiseModelForOverwritingFile(string playerName, AddPlayerModel model)
        {
            model.CanOverwrite = true;
            model.TemporaryPath = Path.GetTempFileName();
            model.PlayerName = playerName;
            System.IO.File.Delete(model.TemporaryPath);
            model.File.SaveAs(model.TemporaryPath);
        }
    }
}
