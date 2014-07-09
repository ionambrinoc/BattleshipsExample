namespace Battleships.Web.Controllers
{
    using Battleships.Web.Models.Home;
    using Battleships.Web.Services;
    using System.Web.Mvc;

    public partial class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public virtual ActionResult Index()
        {
            return View(Views.Index);
        }

        public virtual ActionResult LogIn()
        {
            return View(Views.Index);
        }

        public virtual ActionResult SignUp()
        {
            return View(Views.SignUp);
        }

        [HttpGet]
        public virtual ActionResult Register()
        {
            return View(Views.Index);
        }

        [HttpPost]
        public virtual ActionResult Register(CreateAccountViewModel model)
        {
            var result = userService.AddUser(model.Name, model.Password);
            return RedirectToAction(Actions.Register());
        }
    }
}