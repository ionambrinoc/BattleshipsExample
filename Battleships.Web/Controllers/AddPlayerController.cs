namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Exceptions;
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
            return View(playerRecordsRepository.GetAll());
        }

        [HttpPost]
        public virtual ActionResult Index(FormCollection form)
        {
            var playerFile = Request.Files["file"];
            var playerPicture = Request.Files["picture"];
            if (playerFile != null)
            {
                try
                {
                    var newPlayer = playersUploadService.UploadAndGetPlayerRecord(
                        form.Get("userName"),
                        playerFile, playerPicture,
                        Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]),
                        Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"]));
                    playerRecordsRepository.Add(newPlayer);
                    playerRecordsRepository.SaveContext();
                }
                catch (InvalidPlayerException)
                {
                    ModelState.AddModelError("InvalidPlayerFileError", "The given file is not a valid player file.");
                    return View();
                }
            }

            return RedirectToAction(MVC.Players.Index());
        }
    }
}