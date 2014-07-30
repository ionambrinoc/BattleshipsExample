namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Services;
    using Battleships.Web.Controllers;
    using Battleships.Web.Factories;
    using Battleships.Web.Models.League;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Collections.Generic;

    internal class LeagueControllerTests
    {
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private ILeagueRunner fakeLeagueRunner;
        private ILeaderboardFactory fakeLeaderboardFactory;
        private LeagueController controller;
        private PlayerRecord playerRecordOne;
        private PlayerRecord playerRecordTwo;
        private MatchResult playerOneWin;
        private MatchResult playerTwoWin;
        private IMatchResultsRepository fakeMatchResultsRepository;
        private IBattleshipsPlayerRepository fakeBattleshipsPlayerRepository;

        [SetUp]
        public void SetUp()
        {
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakeLeagueRunner = A.Fake<ILeagueRunner>();
            fakeLeaderboardFactory = A.Fake<ILeaderboardFactory>();
            fakeMatchResultsRepository = A.Fake<IMatchResultsRepository>();
            fakeBattleshipsPlayerRepository = A.Fake<IBattleshipsPlayerRepository>();
            controller = new LeagueController(fakePlayerRecordsRepository, fakeBattleshipsPlayerRepository, fakeLeagueRunner, fakeLeaderboardFactory, fakeMatchResultsRepository);

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
                                          new PlayerStats
                                          {
                                              Id = playerRecordOne.Id,
                                              Name = playerRecordOne.Name,
                                              Wins = 2,
                                              Losses = 1
                                          },
                                          new PlayerStats
                                          {
                                              Id = playerRecordTwo.Id,
                                              Name = playerRecordTwo.Name,
                                              Wins = 1,
                                              Losses = 2
                                          }
                                      };
            A.CallTo(() => fakeLeagueRunner.GetLeagueResults(A<List<IBattleshipsPlayer>>._, A<int>._)).Returns(matchResults);
            A.CallTo(() => fakeLeaderboardFactory.GenerateLeaderboard(matchResults)).Returns(expectedLeaderboard);

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
