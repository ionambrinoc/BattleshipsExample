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

    internal class AddPlayerControllerTests
    {
        private const string TestPlayerUserName = "My Test User";
        private const string TestPlayerName = "My Test Player";
        private const string UserNameField = "userName";
        private const string NameField = "name";
        private PlayerRecord playerRecordOne;
        private PlayerRecord playerRecordTwo;
        private FormCollection formInput;
        private AddPlayerController controller;
        private IPlayerRecordsRepository fakePlayerRecordRepository;
        private IPlayerUploadService fakePlayerUploadService;
        private HttpPostedFileBase fakeFile;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            formInput = new FormCollection { { UserNameField, TestPlayerUserName }, { NameField, TestPlayerName } };
            fakePlayerRecordRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            controller = new AddPlayerController(fakePlayerRecordRepository, fakePlayerUploadService) { ControllerContext = GetFakeControllerContext() };
            playerRecordOne = A.Fake<PlayerRecord>();
            playerRecordTwo = A.Fake<PlayerRecord>();
            playerRecordOne.Id = 1;
            playerRecordTwo.Id = 2;
            playerRecordOne.Name = "Kitten";
            playerRecordTwo.Name = "KittenTwo";
        }

        [Test]
        public void Index_redirects_to_loginview_if_not_authorised()
        {
            //TODO: fix fake user authorization

            // When
            var view = controller.Index();

            // Then
            Assert.That(view, IsMVC.RedirectTo(MVC.Account.LogIn()));
        }

        [Test]
        public void Posting_to_index_redirects_to_index()
        {
            // When
            var result = controller.Index(formInput);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Players.Index()));
        }

        [Test]
        public void Posting_code_file_delegates_to_player_upload_service()
        {
            // When
            controller.Index(formInput);

            // Then
            A.CallTo(() => fakePlayerUploadService.UploadAndGetPlayerRecord(TestPlayerUserName, fakeFile, TestPlayerStore.Directory))
             .MustHaveHappened();
        }

        [Test]
        public void Posting_code_file_saves_new_player()
        {
            // Given
            var fakePlayer = A.Fake<PlayerRecord>();
            A.CallTo(() => fakePlayerUploadService.UploadAndGetPlayerRecord(A<string>._, A<HttpPostedFileBase>._, A<string>._))
             .Returns(fakePlayer);

            // When
            controller.Index(formInput);

            // Then
            A.CallTo(() => fakePlayerRecordRepository.Add(fakePlayer)).MustHaveHappened();
            A.CallTo(() => fakePlayerRecordRepository.SaveContext()).MustHaveHappened();
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