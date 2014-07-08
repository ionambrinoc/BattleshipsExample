namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using System.Web.Mvc;

    public partial class HistoryController : Controller
    {
        private readonly IPastGameRepository pastGameRepo;

        public HistoryController(IPastGameRepository pastGameRepo)
        {
            this.pastGameRepo = pastGameRepo;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(pastGameRepo.GetAll());
        }
    }
}