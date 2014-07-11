namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using System.Web.Mvc;

    public partial class GameResultsController : Controller
    {
        private readonly IGameResultsRepository gameResultsRepo;

        public GameResultsController(IGameResultsRepository gameResultsRepo)
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