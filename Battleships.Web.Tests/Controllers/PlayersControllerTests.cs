namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Factories;
    using Battleships.Runner.Models;
    using Battleships.Runner.Runners;
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Tests.TestHelpers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private IGameLogRepository fakeGameLogRepo;

        [SetUp]
        public void SetUp()
        {
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakeBattleshipsPlayerRepo = A.Fake<IBattleshipsPlayerRepository>();
            fakeHeadToHeadRunner = A.Fake<IHeadToHeadRunner>();
            fakeGameLogRepo = A.Fake<IGameLogRepository>();
            controller = new PlayersController(fakePlayerRecordsRepository, fakeBattleshipsPlayerRepo, fakeHeadToHeadRunner, fakeGameLogRepo);
        }

        [Test]
        public void Index_view_converts_all_player_records_to_player_record_view_models()
        {
            // Given
            A.CallTo(() => fakePlayerRecordsRepository.GetAll()).Returns(new List<PlayerRecord> { new PlayerRecord(), new PlayerRecord() });

            // When
            var result = controller.Index();

            // Then
            Assert.That(result, IsMVC.View(String.Empty));
            controller.ViewData.Model.As<IEnumerable<PlayerRecordViewModel>>().Count().Should().Be(2);
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            // Given
            SetUpPlayers();
            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(fakeWinner, fakeLoser, A<IGameLogFactory>._, fakeGameLogRepo)).Returns(new GameResult(fakeWinner, ResultType.Default));

            // When
            var result = controller.RunGame(WinnerId, LoserId);

            // Then
            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(A<IBattleshipsPlayer>._, A<IBattleshipsPlayer>._, A<IGameLogFactory>._, fakeGameLogRepo)).MustHaveHappened(Repeated.Exactly.Once);
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