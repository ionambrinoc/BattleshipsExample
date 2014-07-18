namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Player;
    using Battleships.Runner.Exceptions;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Runner.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    internal class AddPlayerControllerTests
    {
        private const string UserName = "testUser";
        private AddPlayerController controller;
        private IPlayerRecordsRepository fakePlayerRecordRepository;
        private IPlayerUploadService fakePlayerUploadService;
        private HttpPostedFileBase fakeFile;
        private IBattleshipsPlayer fakeBattleshipsPlayer;
        private PlayerRecord fakePlayerRecord;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;
            fakePlayerRecordRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            controller = new AddPlayerController(fakePlayerRecordRepository, fakePlayerUploadService) { ControllerContext = GetFakeControllerContext() };
            fakeBattleshipsPlayer = A.Fake<IBattleshipsPlayer>();
            fakeFile = A.Fake<HttpPostedFileBase>();
            fakePlayerRecord = A.Fake<PlayerRecord>();
            A.CallTo(() => controller.User.Identity.Name).Returns(UserName);
            A.CallTo(() => fakePlayerUploadService.LoadBattleshipsPlayerFromFile(A<HttpPostedFileBase>.Ignored)).Returns(fakeBattleshipsPlayer);
            A.CallTo(() => fakeBattleshipsPlayer.Name).Returns("testName");
        }

        [Test]
        public void Index_GET_redirects_to_login_view_when_not_authorised()
        {
            // Given
            A.CallTo(() => controller.Request.IsAuthenticated).Returns(false);

            // When
            var result = controller.Index();

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Account.LogIn()));
        }

        [Test]
        public void Index_GET_redirects_to_index_view_when_authorised()
        {
            // Given
            A.CallTo(() => controller.Request.IsAuthenticated).Returns(true);

            // When
            var result = controller.Index();

            // Then
            Assert.That(result, IsMVC.View(""));
            Assert.IsFalse(controller.ViewData.Model.As<AddPlayerModel>().CanOverwrite);
        }

        [Test]
        public void Index_POST_redirects_to_view_when_model_has_an_error()
        {
            // Given
            var model = new AddPlayerModel();
            controller.ModelState.AddModelError("test", "testError");

            // When
            var result = controller.Index(model);

            // Then
            Assert.That(result, IsMVC.View(""));
        }

        [Test]
        public void Index_POST_uploading_a_bot_with_botname_this_user_already_has_updates_model_state_and_returns_view()
        {
            // Given
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile };
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExists(fakeBattleshipsPlayer.Name)).Returns(true);
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExistsForUser(fakeBattleshipsPlayer.Name, UserName)).Returns(true);

            // When
            var result = controller.Index(model);

            // Then
            Assert.That(result, IsMVC.View(""));
            Assert.IsTrue(controller.ViewData.Model.As<AddPlayerModel>().CanOverwrite);
        }

        [Test]
        public void Index_POST_does_not_allow_to_overwrite_other_users_bot()
        {
            // Given
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile };
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExists(fakeBattleshipsPlayer.Name)).Returns(true);
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExistsForUser(fakeBattleshipsPlayer.Name, UserName)).Returns(false);

            // When
            var result = controller.Index(model);

            // Then
            Assert.That(result, IsMVC.View(""));
            Assert.That(controller, HasMVC.ModelLevelErrors());
        }

        [Test]
        public void Index_POST_posting_file_with_new_bot_name_fails_if_bot_file_is_not_valid()
        {
            // Given
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile };
            A.CallTo(() => fakePlayerUploadService.LoadBattleshipsPlayerFromFile(A<HttpPostedFileBase>.Ignored)).Throws(new InvalidPlayerException());

            // When
            var result = controller.Index(model);

            // Then
            Assert.That(result, IsMVC.View(""));
            Assert.That(controller, HasMVC.ModelLevelErrors());
        }

        [Test]
        public void Index_POST_posting_file_with_new_bot_name_succeeds_if_bot_file_is_valid()
        {
            // Given
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile };
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExists(fakeBattleshipsPlayer.Name)).Returns(false);
            A.CallTo(() => fakePlayerUploadService.SaveFileAndGetPlayerRecord("testUser", fakeFile, A<string>.Ignored, fakeBattleshipsPlayer.Name)).Returns(fakePlayerRecord);

            // When
            var result = controller.Index(model);

            // Then
            A.CallTo(() => fakePlayerRecordRepository.Add(fakePlayerRecord)).MustHaveHappened();
            A.CallTo(() => fakePlayerRecordRepository.SaveContext()).MustHaveHappened();
            Assert.That(result, IsMVC.RedirectTo(MVC.Players.Index()));
        }

        [Test]
        public void OverwriteYes_redirects_to_players_index()
        {
            // Given
            var model = new AddPlayerModel { TemporaryPath = Path.GetTempFileName(), PlayerName = "testName" };
            A.CallTo(() => fakePlayerUploadService.GenerateFullPath(model.PlayerName, A<string>.Ignored)).Returns(TestPlayerStore.Directory + "testName.dll");

            // When
            var result = controller.OverwriteYes(model);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Players.Index()));
        }

        [Test]
        public void OverwriteNo_redirects_to_addplayer_index()
        {
            // When
            var result = controller.OverwriteNo();

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.AddPlayer.Index()));
        }

        private ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = A.Fake<HttpContextBase>();
            var fakeRequest = A.Fake<HttpRequestBase>();
            var fileCollection = A.Fake<HttpFileCollectionBase>();
            fakeFile = A.Fake<HttpPostedFileBase>();

            A.CallTo(() => fakeHttpContext.Request).Returns(fakeRequest);
            A.CallTo(() => fakeRequest.Files).Returns(fileCollection);
            A.CallTo(() => fileCollection["file"]).Returns(fakeFile);

            return new ControllerContext(fakeHttpContext, new RouteData(), A.Fake<ControllerBase>());
        }
    }
}