namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Runner.Models;
    using Battleships.Web.Controllers;
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
        public void Login_GET_returns_login_view()
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
            controller.ModelState.Add("testError", new ModelState());
            controller.ModelState.AddModelError("testError", "test");

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
        public void Register_GET_returns_register_view()
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
            Assert.That(controller, HasMVC.ModelLevelErrors());
            Assert.That(result, IsMVC.View(MVC.Account.Views.Register));
        }

        [Test]
        public void Create_with_invalid_model_returns_register_view()
        {
            // Given
            var model = new CreateAccountViewModel();
            controller.ModelState.Add("testError", new ModelState());
            controller.ModelState.AddModelError("testError", "test");

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
    }
}