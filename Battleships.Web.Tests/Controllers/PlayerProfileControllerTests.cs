namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.PlayerProfile;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Configuration;

    [TestFixture]
    public class PlayerProfileControllerTests
    {
        private const int TestPlayerId = 1;
        private PlayerProfileController controller;
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private IPlayerUploadService fakePlayerUploadService;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            controller = new PlayerProfileController(fakePlayerRecordsRepository, fakePlayerUploadService);
        }

        [Test]
        public void Successful_player_delete_redirect_to_manageplayers_index()
        {
            // When
            var result = controller.DeletePlayer(TestPlayerId);

            // Then
            A.CallTo(() => fakePlayerRecordsRepository.DeletePlayerRecordById(TestPlayerId)).MustHaveHappened();
            Assert.That(result, IsMVC.RedirectTo(MVC.ManagePlayers.Index()));
        }

        [Test]
        public void Index_view_converts_player_record_to_player_record_view_model()
        {
            // Given
            A.CallTo(() => fakePlayerRecordsRepository.GetPlayerRecordById(A<int>._)).Returns(new PlayerRecord());

            // When
            var result = controller.Index();

            // Then
            controller.ViewData.Model.Should().Be(A<PlayerRecordViewModel>._);
        }
    }
}