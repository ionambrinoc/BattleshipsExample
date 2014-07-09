namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using System.Web.Mvc;

    public partial class HistoryController : Controller
    {
        private readonly IGameResultsRepository gameResultsRepo;

        public HistoryController(IGameResultsRepository gameResultsRepo)
        {
            this.gameResultsRepo = gameResultsRepo;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(gameResultsRepo.GetAll());
        }
    }
}