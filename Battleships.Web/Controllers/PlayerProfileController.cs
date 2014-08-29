namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Web.Models.PlayerProfile;
    using Battleships.Web.Services;
    using System.Web.Mvc;

    public partial class PlayerProfileController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IPlayerDeletionService playerDeletionService;

        public PlayerProfileController(IPlayerRecordsRepository playerRecordsRepository, IPlayerDeletionService playerDeletionService)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.playerDeletionService = playerDeletionService;
        }

        [HttpGet]
        public virtual ActionResult Index(int id)
        {
            return View(new PlayerRecordViewModel(playerRecordsRepository.GetPlayerRecordById(id)));
        }

        [HttpPost]
        public virtual ActionResult DeletePlayer(int playerId)
        {
            playerDeletionService.DeleteRecordsByPlayerId(playerId);
            return RedirectToAction(MVC.ManagePlayers.Index());
        }
    }
}
