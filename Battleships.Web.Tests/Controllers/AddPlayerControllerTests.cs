namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Player.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [TestFixture]
    public class AddPlayerControllerTests
    {
        private const string UserId = "testId";
        private AddPlayerController controller;
        private IPlayerRecordsRepository fakePlayerRecordRepository;
        private IPlayerUploadService fakePlayerUploadService;
        private HttpPostedFileBase fakeFile;
        private IBattleshipsBot fakeBot;
        private HttpPostedFileBase fakePicture;
        private PlayerRecord fakePlayerRecord;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;
            ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"] = TestPlayerStore.Directory;
            fakePlayerRecordRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            controller = new AddPlayerController(fakePlayerRecordRepository, fakePlayerUploadService) { ControllerContext = GetFakeControllerContext() };
            fakeBot = A.Fake<IBattleshipsBot>();
            fakeFile = A.Fake<HttpPostedFileBase>();
            fakePlayerRecord = A.Fake<PlayerRecord>();
            A.CallTo(() => controller.User.Identity).Returns(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, UserId) }));
            A.CallTo(() => fakePlayerUploadService.LoadBotFromFile(A<HttpPostedFileBase>.Ignored)).Returns(fakeBot);
            A.CallTo(() => fakeBot.Name).Returns("testName");
        }

        [TestCaseSource("ValidFormats")]
        public void Picture_with_valid_format_is_accepted(string format)
        {
            // Given
            A.CallTo(() => fakePicture.ContentType).Returns(format);
            var model = new AddPlayerModel { Picture = fakePicture };

            // When
            var result = controller.Index(model);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Players.Index()));
        }

        [TestCaseSource("InvalidFormats")]
        public void Picture_with_invalid_format_is_not_accepted(string format)
        {
            // Given
            A.CallTo(() => fakePicture.ContentType).Returns(format);
            var model = new AddPlayerModel { Picture = fakePicture };

            // When
            var result = controller.Index(model);

            // Then
            Assert.That(result, IsMVC.View(""));
        }

        [Test]
        public void Picture_is_not_required()
        {
            // Given
            var model = new AddPlayerModel { File = fakeFile };

            // When
            var result = controller.Index(model);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Players.Index()));
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
        public void Index_POST_uploading_an_existing_bot_belonging_to_the_user_marks_model_as_overwriting_and_returns_view()
        {
            // Given
            var fakePlayer = A.Fake<PlayerRecord>();
            A.CallTo(() => fakePlayerUploadService.UploadAndGetPlayerRecord(A<string>._, A<HttpPostedFileBase>._, A<HttpPostedFileBase>._, A<string>._))
             .Returns(fakePlayer);
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile };
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExists(fakeBot.Name)).Returns(true);
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExistsForUser(fakeBot.Name, UserId)).Returns(true);

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
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExists(fakeBot.Name)).Returns(true);
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExistsForUser(fakeBot.Name, UserId)).Returns(false);

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
            A.CallTo(() => fakePlayerUploadService.LoadBotFromFile(A<HttpPostedFileBase>.Ignored)).Throws(new InvalidPlayerException());

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
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile, Picture = fakePicture };
            A.CallTo(() => fakePlayerRecordRepository.PlayerNameExists(fakeBot.Name)).Returns(false);
            A.CallTo(() => fakePlayerUploadService.UploadAndGetPlayerRecord(UserId, fakeFile, fakePicture, fakeBot.Name)).Returns(fakePlayerRecord);
            A.CallTo(() => fakePicture.ContentType).Returns("image/jpg");

            // When
            var result = controller.Index(model);

            // Then
            A.CallTo(() => fakePlayerRecordRepository.Add(fakePlayerRecord)).MustHaveHappened();
            A.CallTo(() => fakePlayerRecordRepository.SaveContext()).MustHaveHappened();
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

        [Test]
        public void OverwriteYes_redirects_to_players_index()
        {
            // Given
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile, Picture = fakePicture };

            // When
            var result = controller.OverwriteYes(model);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Players.Index()));
        }

        [Test]
        public void OverwriteYes_marks_player_as_updated_and_saves_context()
        {
            // Given
            var model = new AddPlayerModel { CanOverwrite = false, File = fakeFile, Picture = fakePicture };

            // When
            controller.OverwriteYes(model);

            // Then
            A.CallTo(() => fakePlayerRecordRepository.MarkPlayerAsUpdated(model.PlayerName)).MustHaveHappened();
            A.CallTo(() => fakePlayerRecordRepository.SaveContext()).MustHaveHappened();
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<string> ValidFormats()
        {
            yield return "image/jpg";
            yield return "image/gif";
            yield return "image/png";
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<string> InvalidFormats()
        {
            yield return "application/x-zip-compressed";
            yield return "text/html";
            yield return "application/vnd.ms-excel";
        }

        private ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = A.Fake<HttpContextBase>();
            var fakeRequest = A.Fake<HttpRequestBase>();
            var fileCollection = A.Fake<HttpFileCollectionBase>();
            fakeFile = A.Fake<HttpPostedFileBase>();
            fakePicture = A.Fake<HttpPostedFileBase>();

            A.CallTo(() => fakeHttpContext.Request).Returns(fakeRequest);
            A.CallTo(() => fakeRequest.Files).Returns(fileCollection);
            A.CallTo(() => fileCollection["file"]).Returns(fakeFile);
            A.CallTo(() => fileCollection["picture"]).Returns(fakePicture);

            return new ControllerContext(fakeHttpContext, new RouteData(), A.Fake<ControllerBase>());
        }
    }
}