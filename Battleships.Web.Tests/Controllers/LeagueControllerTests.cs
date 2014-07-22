namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Player;
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.League;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Collections.Generic;

    internal class LeagueControllerTests
    {
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private ILeagueRunner fakeLeagueRunner;
        private ILeaderboardViewModel fakeLeaderboardViewModel;
        private LeagueController controller;
        private PlayerRecord playerRecordOne;
        private PlayerRecord playerRecordTwo;
        private MatchResult playerOneWin;
        private MatchResult playerTwoWin;

        [SetUp]
        public void SetUp()
        {
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakeLeagueRunner = A.Fake<ILeagueRunner>();
            fakeLeaderboardViewModel = A.Fake<ILeaderboardViewModel>();
            controller = new LeagueController(fakePlayerRecordsRepository, fakeLeagueRunner, fakeLeaderboardViewModel);

            playerRecordOne = A.Fake<PlayerRecord>();
            playerRecordTwo = A.Fake<PlayerRecord>();

            playerOneWin = SetUpMatchResult(playerRecordOne, playerRecordTwo);
            playerTwoWin = SetUpMatchResult(playerRecordTwo, playerRecordOne);
        }

        [Test]
        public void Run_league_returns_json_results()
        {
            // Given
            var matchResults = new List<MatchResult> { playerOneWin, playerOneWin, playerTwoWin };
            var expectedLeaderboard = new List<PlayerStats>
                                      {
                                          new PlayerStats(playerRecordOne, 2, 5, 1),
                                          new PlayerStats(playerRecordTwo, 1, 4, 2)
                                      };
            A.CallTo(() => fakeLeagueRunner.GetLeagueResults(A<List<IBattleshipsPlayer>>._, A<int>._)).Returns(matchResults);
            A.CallTo(() => fakeLeaderboardViewModel.GenerateLeaderboard(matchResults)).Returns(expectedLeaderboard);

            // When
            var result = controller.RunLeague();

            // Then
            Assert.That(result, IsMVC.Json(expectedLeaderboard));
        }

        private MatchResult SetUpMatchResult(PlayerRecord winner, PlayerRecord loser)
        {
            var matchResult = A.Fake<MatchResult>();
            matchResult.Winner = winner;
            matchResult.Loser = loser;
            matchResult.WinnerWins = 2;
            matchResult.LoserWins = 1;

            return matchResult;
        }
    }
}