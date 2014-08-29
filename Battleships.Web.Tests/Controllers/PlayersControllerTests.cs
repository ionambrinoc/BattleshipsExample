namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Models;
    using Battleships.Runner.Runners;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Web.Mvc;

    [TestFixture]
    public class PlayersControllerTests
    {
        private const string WinnerName = "Winner name";
        private const string LoserName = "Loser name";
        private const int WinnerId = 12;
        private const int LoserId = 34;

        private IHeadToHeadRunner fakeHeadToHeadRunner;
        private IBattleshipsPlayer fakeWinner;
        private IBattleshipsPlayer fakeLoser;
        private IBattleshipsPlayerRepository fakeBattleshipsPlayerRepo;

        private PlayersController controller;
        private IPlayerRecordsRepository fakePlayerRecordsRepository;

        [SetUp]
        public void SetUp()
        {
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakeBattleshipsPlayerRepo = A.Fake<IBattleshipsPlayerRepository>();
            fakeHeadToHeadRunner = A.Fake<IHeadToHeadRunner>();
            controller = new PlayersController(fakePlayerRecordsRepository, fakeBattleshipsPlayerRepo, fakeHeadToHeadRunner);
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            // Given
            SetUpPlayers();
            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(fakeWinner, fakeLoser, A<ILogger>._)).Returns(new GameResult(fakeWinner, ResultType.Default));

            // When
            var result = controller.RunGame(WinnerId, LoserId);

            // Then
            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(A<IBattleshipsPlayer>._, A<IBattleshipsPlayer>._, A<ILogger>._)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.IsInstanceOf<JsonResult>(result);
            result.GetProperty("winnerName").Should().Be(WinnerName);
            result.GetProperty("resultType").Should().Be((int)ResultType.Default);
        }

        private void SetUpPlayers()
        {
            fakeWinner = A.Fake<IBattleshipsPlayer>();
            fakeLoser = A.Fake<IBattleshipsPlayer>();

            PlayerHasIdAndName(fakeWinner, WinnerId, WinnerName);
            PlayerHasIdAndName(fakeLoser, LoserId, LoserName);
        }

        private void PlayerHasIdAndName(IBattleshipsPlayer battleshipsPlayer, int id, string name)
        {
            A.CallTo(() => battleshipsPlayer.Name).Returns(name);
            A.CallTo(() => fakeBattleshipsPlayerRepo.GetBattleshipsPlayerFromPlayerRecordId(id)).Returns(battleshipsPlayer);
        }
    }
}
