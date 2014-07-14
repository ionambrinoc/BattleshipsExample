namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Services;
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    public partial class AddPlayerController : Controller
    {
        private readonly IPlayersRepository playersRepository;
        private readonly IPlayerUploadService playersUploadService;

        public AddPlayerController(IPlayersRepository playersRepository, IPlayerUploadService playersUploadService)
        {
            this.playersRepository = playersRepository;
            this.playersUploadService = playersUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playersRepository.GetAll());
        }

        [HttpPost]
        public virtual ActionResult Index(FormCollection form)
        {
            var playerFile = Request.Files["file"];
            if (playerFile != null)
            {
                try
                {
                    var newPlayer = playersUploadService.UploadAndGetPlayer(
                        form.Get("userName"),
                        playerFile,
                        Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]));

                    playersRepository.Add(newPlayer);
                    playersRepository.SaveContext();
                }
                catch
                {
                    ModelState.AddModelError("InvalidPlayerFileError", "The given file is not a valid player file.");
                    return View();
                }
            }

            return RedirectToAction(MVC.Players.Index());
        }
    }
}