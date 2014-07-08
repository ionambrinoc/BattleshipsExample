namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Services;
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayersRepository playersRepo;
        private readonly IPlayerUploadService playersUploadService;

        public PlayersController(IPlayersRepository playerRepo, IPlayerUploadService playerUploadService)
        {
            playersRepo = playerRepo;
            playersUploadService = playerUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playersRepo.GetAll());
        }

        [HttpPost]
        public virtual ActionResult Index(FormCollection form)
        {
            var codeFile = Request.Files["file"];
            if (codeFile != null)
            {
                var newPlayer = playersUploadService.UploadAndGetPlayer(
                    form.Get("userName"),
                    form.Get("name"),
                    codeFile,
                    Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]));
                playersRepo.Add(newPlayer);
                playersRepo.SaveContext();
            }

            return RedirectToAction(Actions.Index());
        }
    }
}