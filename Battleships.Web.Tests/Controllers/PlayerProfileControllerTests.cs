namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.PlayerProfile;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Web.Mvc;

    [TestFixture]
    public class PlayerProfileControllerTests
    {
        private const int TestPlayerId = 1;
        private PlayerProfileController controller;
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private IPlayerUploadService fakePlayerUploadService;
        private IPlayerDeletionService fakePlayerDeletionService;

        [SetUp]
        public void SetUp()
        {
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            fakePlayerDeletionService = A.Fake<IPlayerDeletionService>();
            controller = new PlayerProfileController(fakePlayerRecordsRepository, fakePlayerDeletionService);
        }

        [Test]
        public void Successful_player_delete_redirect_to_manageplayers_index()
        {
            // When
            var result = controller.DeletePlayer(TestPlayerId);

            // Then
            A.CallTo(() => fakePlayerDeletionService.DeleteRecordsByPlayerId(TestPlayerId)).MustHaveHappened();
            Assert.That(result, IsMVC.RedirectTo(MVC.ManagePlayers.Index()));
        }

        [Test]
        public void Index_view_converts_player_record_to_player_record_view_model()
        {
            // Given
            A.CallTo(() => fakePlayerRecordsRepository.GetPlayerRecordById(A<int>._)).Returns(new PlayerRecord { Id = 1, Name = "Foo", PictureFileName = "Foo.jpg" });

            // When
            var viewResult = controller.Index(TestPlayerId) as ViewResult;

            // Then
            Assert.NotNull(viewResult);
            viewResult.ViewData.Model.Should().BeOfType<PlayerRecordViewModel>();
        }
    }
}
