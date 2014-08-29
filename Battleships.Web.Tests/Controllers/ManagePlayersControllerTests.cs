namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Player.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [TestFixture]
    public class ManagePlayersControllerTests
    {
        private ManagePlayersController controller;
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private IPlayerDeletionService fakePlayerDeletionService;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestDirectory.TestPlayerStore;
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerDeletionService = A.Fake<IPlayerDeletionService>();
            controller = new ManagePlayersController(fakePlayerRecordsRepository, fakePlayerUploadService, fakePlayerDeletionService) { ControllerContext = GetFakeControllerContext() };
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
            A.CallTo(() => fakePlayerDeletionService.DeleteRecordsByPlayerId(TestPlayerId)).DoesNothing();

            // When
            var result = controller.DeletePlayer(TestPlayerId);

            // Then
            A.CallTo(() => fakePlayerDeletionService.DeleteRecordsByPlayerId(TestPlayerId)).MustHaveHappened();
            Assert.That(result, IsMVC.RedirectTo(MVC.ManagePlayers.Index()));
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