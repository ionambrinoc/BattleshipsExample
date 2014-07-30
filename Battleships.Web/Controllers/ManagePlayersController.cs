namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Web.Services;
    using Microsoft.AspNet.Identity;
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
            return View(playerRecordsRepository.GetAllForUserId(User.Identity.GetUserId()));
        }

        [HttpPost]
        public virtual ActionResult DeletePlayer(int playerId)
        {
            var player = playerRecordsRepository.GetPlayerRecordById(playerId);
            playerRecordsRepository.DeletePlayerRecordById(playerId);
            playerUploadService.DeletePlayer(player.Name, player.PictureFileName);
            return RedirectToAction(MVC.ManagePlayers.Index());
        }
    }
}
