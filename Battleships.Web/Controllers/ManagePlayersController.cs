namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Controllers.Helpers;
    using Battleships.Web.Services;
    using System.Web.Mvc;

    public partial class ManagePlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IPlayerUploadService playerUploadService;

        public ManagePlayersController(IPlayerRecordsRepository playerRecordsRepository, IPlayerUploadService playerUploadService)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.playerUploadService = playerUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAllForUserName(User.Identity.Name));
        }

        [HttpPost]
        public virtual ActionResult DeletePlayer(int playerId)
        {
            var playerName = playerRecordsRepository.GetPlayerRecordById(playerId).Name;
            playerRecordsRepository.DeletePlayerRecordById(playerId);
            System.IO.File.Delete(playerUploadService.GenerateFullPath(playerName, this.GetUploadDirectoryPath()));
            return RedirectToAction(MVC.ManagePlayers.Index());
        }
    }
}