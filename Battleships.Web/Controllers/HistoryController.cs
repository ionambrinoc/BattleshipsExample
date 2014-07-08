namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Runner.Services;
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    public partial class HistoryController : Controller
    {
        private readonly IPastGameRepository pastGameRepo;
        // private readonly IKittenUploadService kittenUploadService;

        public HistoryController(IPastGameRepository pastGameRepo /* KittenUploadService kittenUploadService*/)
        {
            this.pastGameRepo = pastGameRepo;
            // this.kittenUploadService = kittenUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(pastGameRepo.GetAll());
        }

        /*   public class HistoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    } */
    }
} 