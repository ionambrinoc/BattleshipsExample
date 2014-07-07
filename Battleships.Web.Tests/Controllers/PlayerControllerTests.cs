namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Runner.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [TestFixture]
    public class PlayerControllerTests
    {
        private const string TestPlayerUserName = "My Test Kitten";
        private const string TestPlayerBotName = "My Test KittenBot";
        private const string UserNameField = "userName";
        private const string BotNameField = "botName";
        private PlayersController controller;
        private IPlayersRepository fakePlayerRepo;
        private IPlayerUploadService fakePlayerUploadService;
        private HttpPostedFileBase fakeCodeFile;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            fakePlayerRepo = A.Fake<IPlayersRepository>();
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            controller = new PlayersController(fakePlayerRepo, fakePlayerUploadService) { ControllerContext = GetFakeControllerContext() };
        }

        [Test]
        public void Index_returns_index_view()
        {
            // When
            var view = controller.Index();

            // Then
            Assert.That(view, IsMVC.View(""));
        }

        [Test]
        public void Posting_to_index_redirects_to_index()
        {
            // When
            var result = controller.Index(new FormCollection { { UserNameField, TestPlayerUserName }, { BotNameField, TestPlayerBotName } });

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Players.Index()));
        }

        [Test]
        public void Posting_code_file_delegates_to_player_upload_service()
        {
            // When
            controller.Index(new FormCollection { { UserNameField, TestPlayerUserName }, { BotNameField, TestPlayerBotName } });

            // Then
            A.CallTo(() => fakePlayerUploadService.UploadAndGetPlayer(TestPlayerUserName, TestPlayerBotName, fakeCodeFile, TestPlayerStore.Directory))
             .MustHaveHappened();
        }

        [Test]
        public void Posting_code_file_saves_new_player()
        {
            // Given
            var fakePlayer = A.Fake<Player>();
            A.CallTo(() => fakePlayerUploadService.UploadAndGetPlayer(A<string>._, A<string>._, A<HttpPostedFileBase>._, A<string>._))
             .Returns(fakePlayer);

            // When
            controller.Index(new FormCollection { { UserNameField, TestPlayerUserName }, { BotNameField, TestPlayerBotName } });

            // Then
            A.CallTo(() => fakePlayerRepo.Add(fakePlayer)).MustHaveHappened();
            A.CallTo(() => fakePlayerRepo.SaveContext()).MustHaveHappened();
        }

        private ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = A.Fake<HttpContextBase>();
            var fakeRequest = A.Fake<HttpRequestBase>();
            var fileCollection = A.Fake<HttpFileCollectionBase>();
            fakeCodeFile = A.Fake<HttpPostedFileBase>();

            A.CallTo(() => fakeHttpContext.Request).Returns(fakeRequest);
            A.CallTo(() => fakeRequest.Files).Returns(fileCollection);
            A.CallTo(() => fileCollection["codeFile"]).Returns(fakeCodeFile);

            return new ControllerContext(fakeHttpContext, new RouteData(), A.Fake<ControllerBase>());
        }
    }
}