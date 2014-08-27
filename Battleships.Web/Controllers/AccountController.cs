namespace Battleships.Web.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Web.Models.Account;
    using Battleships.Web.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public partial class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IAuthenticationManager authenticationManager;

        public AccountController(IUserService userService, IAuthenticationManager authenticationManager)
        {
            this.userService = userService;
            this.authenticationManager = authenticationManager;
        }

        [HttpGet]
        public virtual ActionResult LogOff()
        {
            authenticationManager.SignOut();
            return RedirectToAction(MVC.Home.Index());
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult LogIn()
        {
            return View(Views.Login);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult LogIn(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userService.Find(model.Name, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(Views.Login);
                }
                SignIn(user);
                return RedirectToAction(MVC.Home.Index());
            }

            return View(Views.Login);
        }

        [HttpGet]
        public virtual ActionResult Register()
        {
            return View(Views.Register);
        }

        [HttpGet]
        public virtual ActionResult ChangePasswordPage()
        {
            return View(Views.ChangePasswordPage);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult ChangePassword()
        {
            return View(Views.ChangePasswordPage);
        }

        [HttpPost]
        public virtual ActionResult Register(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = userService.AddUser(model.Name, model.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault());
                    return View(Views.Register);
                }

                var user = userService.Find(model.Name, model.Password);
                SignIn(user);
                return RedirectToAction(MVC.Home.Index());
            }
            return View(Views.Register);
        }

        public virtual JsonResult IsUserNameAvailable(string name)
        {
            if (userService.DoesUserExist(name))
            {
                return Json(String.Format("Username {0} is already taken", name), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void SignIn(User user)
        {
            var identity = userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
        }
    }
}