namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using System.Web.Mvc;

    public partial class KittensController : Controller
    {
        private readonly IKittensRepository kittensRepo;

        public KittensController(IKittensRepository kittensRepo)
        {
            this.kittensRepo = kittensRepo;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Index(FormCollection form)
        {
            kittensRepo.Add(new Kitten { Name = form.Get("kittenName") });

            return RedirectToAction(Actions.Index());
        }
    }
}
