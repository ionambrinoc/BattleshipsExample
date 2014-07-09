namespace Battleships.Web.Controllers
{
    using Battleships.Web.Models.Home;
    using Battleships.Web.Services;
    using System.Linq;
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

        [HttpGet]
        public virtual ActionResult LogIn()
        {
            return View(Views.Index);
        }

        [HttpPost]
        public virtual ActionResult LogIn(LogInViewModel model)
        {
            if (ModelState.IsValid) {}

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
            if (ModelState.IsValid)
            {
                bool signUpFailed;
                var errormessage = "";

                var result = userService.AddUser(model.Name, model.Password);
                if (result.Succeeded)
                {
                    signUpFailed = false;
                }
                else
                {
                    signUpFailed = true;
                    errormessage = result.Errors.FirstOrDefault();
                    ModelState.AddModelError("SqlError", errormessage);
                    return View(Views.SignUp);
                }

                return RedirectToAction(Actions.Index());
            }

            return View(Views.SignUp);
        }
    }
}