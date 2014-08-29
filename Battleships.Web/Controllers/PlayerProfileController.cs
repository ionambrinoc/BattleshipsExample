namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Web.Models.PlayerProfile;
    using Battleships.Web.Services;
    using System.Web.Mvc;

    public partial class PlayerProfileController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IPlayerUploadService playerUploadService;

        public PlayerProfileController(IPlayerRecordsRepository playerRecordsRepository, IPlayerUploadService playerUploadService)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.playerUploadService = playerUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index(int id)
        {
            return View(new PlayerRecordViewModel(playerRecordsRepository.GetPlayerRecordById(id)));
        }

        [HttpPost]
        public virtual ActionResult DeletePlayer(int id)
        {
            var player = playerRecordsRepository.GetPlayerRecordById(id);
            playerRecordsRepository.DeletePlayerRecordById(id);
            playerUploadService.DeletePlayer(player.Name, player.PictureFileName);
            return RedirectToAction(MVC.ManagePlayers.Index());
        }
    }
}