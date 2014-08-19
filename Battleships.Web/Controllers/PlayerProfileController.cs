namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using System.Web.Mvc;

    public partial class PlayerProfileController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;

        public PlayerProfileController(IPlayerRecordsRepository playerRecordsRepository)
        {
            this.playerRecordsRepository = playerRecordsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index(int id)
        {
            return View(playerRecordsRepository.GetPlayerRecordById(id));
        }
    }
}