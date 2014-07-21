namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Runner.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Controllers.Helpers;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class ManagePlayersControllerTests
    {
        private ManagePlayersController controller;
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private IPlayerUploadService fakePlayerUploadService;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            controller = new ManagePlayersController(fakePlayerRecordsRepository, fakePlayerUploadService) { ControllerContext = GetFakeControllerContext() };
        }

        [Test]
        public void Index_returns_index_view()
        {
            // When
            var result = controller.Index();

            // Then
            Assert.That(result, IsMVC.View(""));
        }

        [Test]
        public void Successful_player_delete_redirect_to_manageplayers_index()
        {
            // Given
            A.CallTo(() => fakePlayerRecordsRepository.DeletePlayerRecordById(0)).DoesNothing();
            var temporaryPath = Path.GetTempFileName();
            A.CallTo(() => fakePlayerUploadService.GenerateFullPath(A<string>.Ignored, controller.GetUploadDirectoryPath())).Returns(temporaryPath);

            // When
            var result = controller.DeletePlayer(0);

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.ManagePlayers.Index()));
        }

        [Test]
        public void Unsuccessful_player_delete_adds_model_error_and_returns_index_view()
        {
            // Given
            A.CallTo(() => fakePlayerRecordsRepository.DeletePlayerRecordById(0)).Throws(new Exception());

            // When
            var result = controller.DeletePlayer(0);

            // Then
            Assert.That(controller, HasMVC.ModelLevelErrors());
            Assert.That(result, IsMVC.View(MVC.ManagePlayers.Views.Index));
        }

        private ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = A.Fake<HttpContextBase>();
            var fakeRequest = A.Fake<HttpRequestBase>();
            var fileCollection = A.Fake<HttpFileCollectionBase>();

            A.CallTo(() => fakeHttpContext.Request).Returns(fakeRequest);
            A.CallTo(() => fakeRequest.Files).Returns(fileCollection);

            return new ControllerContext(fakeHttpContext, new RouteData(), A.Fake<ControllerBase>());
        }
    }
}