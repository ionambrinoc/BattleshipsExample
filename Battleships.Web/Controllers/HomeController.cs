namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Models;
    using Battleships.Web.Models.Home;
    using Battleships.Web.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using System.Linq;
    using System.Web.Mvc;

    public partial class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly IAuthenticationManager authenticationManager;

        public HomeController(IUserService userService, IAuthenticationManager authenticationManager)
        {
            this.userService = userService;
            this.authenticationManager = authenticationManager;
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
            if (ModelState.IsValid)
            {
                var user = userService.Find(model.Name, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("SqlError", "Invalid username or password");
                }
                SignIn(user);
            }

            return RedirectToAction(Actions.Index());
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

        private void SignIn(User user)
        {
            var identity = userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(identity);
        }
    }
}