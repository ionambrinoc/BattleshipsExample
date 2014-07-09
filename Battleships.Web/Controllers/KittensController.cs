namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Services;
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    public partial class KittensController : Controller
    {
        private readonly IKittensRepository kittensRepo;
        private readonly IKittenUploadService kittenUploadService;

        public KittensController(IKittensRepository kittensRepo, IKittenUploadService kittenUploadService)
        {
            this.kittensRepo = kittensRepo;
            this.kittenUploadService = kittenUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(kittensRepo.GetAll());
        }

        [HttpPost]
        public virtual ActionResult Index(FormCollection form)
        {
            var imageFile = Request.Files["kittenFile"];
            if (imageFile != null)
            {
                var newKitten = kittenUploadService.UploadAndGetKitten(
                    form.Get("kittenName"),
                    imageFile,
                    Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]));
                kittensRepo.Add(newKitten);
                kittensRepo.SaveContext();
            }
            return RedirectToAction(Actions.Index());
        }
    }
}
