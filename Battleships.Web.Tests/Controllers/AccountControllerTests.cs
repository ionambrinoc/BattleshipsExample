namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Web.Controllers;
    using Battleships.Web.Helper;
    using Battleships.Web.Models.Account;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using NUnit.Framework;
    using System;
    using System.Security.Claims;
    using System.Web.Mvc;

    [TestFixture]
    public class AccountControllerTests
    {
        private const string UserName = "testUser";
        private AccountController controller;
        private IUserService userService;
        private IAuthenticationManager authenticationManager;

        [SetUp]
        public void SetUp()
        {
            userService = A.Fake<IUserService>();
            authenticationManager = A.Fake<IAuthenticationManager>();
            controller = new AccountController(userService, authenticationManager);
        }

        [Test]
        public void Log_off_signs_out_and_redirects_to_home()
        {
            // When
            var result = controller.LogOff();

            // Then
            A.CallTo(() => authenticationManager.SignOut()).MustHaveHappened();
            Assert.That(result, IsMVC.RedirectTo(MVC.Home.Index()));
        }

        [Test]
        public void Login_returns_login_view()
        {
            // When
            var result = controller.LogIn();

            // Then
            Assert.That(result, IsMVC.View(MVC.Account.Views.Login));
        }

        [Test]
        public void Login_invalid_model_returns_login_view()
        {
            // Given
            var model = new LogInViewModel();
            AddModelErrorToController();

            // When
            var result = controller.LogIn(model);

            // Then
            Assert.That(result, IsMVC.View(MVC.Account.Views.Login));
        }

        [Test]
        public void Successful_login_redirects_to_home()
        {
            // Given
            var model = new LogInViewModel();
            var user = new User();
            var identity = new ClaimsIdentity();

            A.CallTo(() => userService.Find(model.Name, model.Password)).Returns(user);
            A.CallTo(() => userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie)).Returns(identity);

            // When
            var result = controller.LogIn(model);

            // Then
            A.CallTo(() => authenticationManager.SignIn(A<AuthenticationProperties>.That.Matches(ap => ap.IsPersistent == false), identity)).MustHaveHappened();
            Assert.That(result, IsMVC.RedirectTo(MVC.Home.Index()));
        }

        [Test]
        public void Login_with_unknown_credentials_adds_model_error_and_returns_view()
        {
            // Given
            var model = new LogInViewModel();
            A.CallTo(() => userService.Find(model.Name, model.Password)).Returns(null);

            // When
            var result = controller.LogIn(model);

            // Then
            Assert.That(controller, HasMVC.ModelLevelErrors());
            Assert.That(result, IsMVC.View(MVC.Account.Views.Login));
        }

        [Test]
        public void ChangePassword_returns_change_password_view()
        {
            // When
            var result = controller.ChangePassword();

            // Then
            Assert.That(result, IsMVC.View(MVC.Account.Views.ChangePassword));
        }

        [Test]
        public void Successful_password_change_redirects_to_home_and_has_popup()
        {
            // Given
            var model = SetupChangePasswordTestAndGetModel(IdentityResult.Success);
            // When
            var result = controller.ChangePassword(model);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Home.Index()));
            Assert.That(controller.TempData.GetPopup().CssClass == "alert-success");
        }

        [Test]
        public void Unsuccessful_password_change_returns_same_view_and_has_popup()
        {
            //Given
            var model = SetupChangePasswordTestAndGetModel(IdentityResult.Failed());

            // When
            var result = controller.ChangePassword(model);

            // Then
            Assert.That(result, IsMVC.View(MVC.Account.Views.ChangePassword));
            Assert.That(controller.TempData.GetPopup().CssClass == "alert-danger");
        }

        [Test]
        public void Password_change_with_invalid_model_returns_change_password_view()
        {
            // Given
            var model = new ChangePasswordViewModel();
            AddModelErrorToController();

            // When
            var result = controller.ChangePassword(model);

            // Then
            Assert.That(result, IsMVC.View(MVC.Account.Views.ChangePassword));
        }

        [Test]
        public void Register_returns_register_view()
        {
            // When
            var result = controller.Register();

            // Then
            Assert.That(result, IsMVC.View(MVC.Account.Views.Register));
        }

        [Test]
        public void Can_create_account()
        {
            // Given
            var model = new CreateAccountViewModel();
            var identityResult = IdentityResult.Success;
            A.CallTo(() => userService.AddUser(model.Name, model.Password)).Returns(identityResult);

            // When
            var result = controller.Register(model);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Home.Index()));
        }

        [Test]
        public void Automatic_signin_when_created_account()
        {
            // Given
            var model = new CreateAccountViewModel();
            var identityResult = IdentityResult.Success;
            A.CallTo(() => userService.AddUser(model.Name, model.Password)).Returns(identityResult);

            var user = new User();
            var identity = new ClaimsIdentity();
            A.CallTo(() => userService.Find(model.Name, model.Password)).Returns(user);
            A.CallTo(() => userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie)).Returns(identity);

            // When
            controller.Register(model);

            // Then
            A.CallTo(() => authenticationManager.SignIn(A<AuthenticationProperties>.That.Matches(ap => ap.IsPersistent == false), identity)).MustHaveHappened();
        }

        [Test]
        public void Create_account_failure_is_handled()
        {
            // Given
            var model = new CreateAccountViewModel();
            var identityResult = IdentityResult.Failed();
            A.CallTo(() => userService.AddUser(model.Name, model.Password)).Returns(identityResult);

            // When
            var result = controller.Register(model);

            // Then
            Assert.That(controller.TempData.GetPopup().CssClass == "alert-danger");
            Assert.That(controller, HasMVC.ModelLevelErrors());
            Assert.That(result, IsMVC.View(MVC.Account.Views.Register));
        }

        [Test]
        public void Create_with_invalid_model_returns_register_view()
        {
            // Given
            var model = new CreateAccountViewModel();
            AddModelErrorToController();

            // When
            var result = controller.Register(model);

            // Then
            Assert.That(result, IsMVC.View(MVC.Account.Views.Register));
        }

        [Test]
        public void Username_is_available_if_there_is_no_other_user_with_the_same_name()
        {
            // Given
            A.CallTo(() => userService.DoesUserExist(UserName)).Returns(false);

            // When
            var result = controller.IsUserNameAvailable(UserName);

            // Then
            Assert.That(result, IsMVC.Json(true));
            result.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
        }

        [Test]
        public void If_username_is_not_available_correct_error_message_is_displayed()
        {
            // Given
            A.CallTo(() => userService.DoesUserExist(UserName)).Returns(true);

            // When
            var result = controller.IsUserNameAvailable(UserName);

            // Then
            Assert.That(result, IsMVC.Json(String.Format("Username {0} is already taken", UserName)));
            result.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
        }

        private void AddModelErrorToController()
        {
            controller.ModelState.Add("testError", new ModelState());
            controller.ModelState.AddModelError("testError", "test");
        }

        private ChangePasswordViewModel SetupChangePasswordTestAndGetModel(IdentityResult result)
        {
            var model = new ChangePasswordViewModel();
            var userId = new User().Id;
            A.CallTo(() => authenticationManager.User.Identity).Returns(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }));
            A.CallTo(() => userService.ChangePassword(userId, model.CurrentPassword, model.NewPassword)).Returns(result);
            return model;
        }
    }
}