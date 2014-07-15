namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using System.Web.Mvc;

    public partial class HistoryController : Controller
    {
        private readonly IMatchResultsRepository matchResultsRepo;

        public HistoryController(IMatchResultsRepository matchResultsRepo)
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