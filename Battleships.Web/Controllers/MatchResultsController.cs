namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using System.Web.Mvc;

    public partial class MatchResultsController : Controller
    {
        private readonly IMatchResultsRepository matchResultsRepo;

        public MatchResultsController(IMatchResultsRepository matchResultsRepo)
        {
            this.matchResultsRepo = matchResultsRepo;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(matchResultsRepo.GetAll());
        }
    }
}