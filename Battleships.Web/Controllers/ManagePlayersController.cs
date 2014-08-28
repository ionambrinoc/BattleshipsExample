namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Microsoft.AspNet.Identity;
    using System.Web.Mvc;

    public partial class ManagePlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;

        public ManagePlayersController(IPlayerRecordsRepository playerRecordsRepository)
        {
            this.playerRecordsRepository = playerRecordsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAllForUserId(User.Identity.GetUserId()));
        }
    }
}