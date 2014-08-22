﻿namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Runner.Runners;
    using Battleships.Web.Controllers;
    using Battleships.Web.Factories;
    using Battleships.Web.Models.League;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class LeagueControllerTests
    {
        private const int NumberOfRounds = 100;
        private readonly DateTime earlierDate = new DateTime(2001, 1, 1);
        private readonly DateTime middleDate = new DateTime(2001, 6, 1);
        private readonly DateTime laterDate = new DateTime(2002, 1, 1);
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
        private ILeagueRecordsRepository fakeLeagueRecordsRepository;

        [SetUp]
        public void SetUp()
        {
            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakeLeagueRunner = A.Fake<ILeagueRunner>();
            fakeLeaderboardFactory = A.Fake<ILeaderboardFactory>();
            fakeMatchResultsRepository = A.Fake<IMatchResultsRepository>();
            fakeBattleshipsPlayerRepository = A.Fake<IBattleshipsPlayerRepository>();
            fakeLeagueRecordsRepository = A.Fake<ILeagueRecordsRepository>();

            controller = new LeagueController(fakePlayerRecordsRepository, fakeBattleshipsPlayerRepository, fakeLeagueRunner, fakeLeaderboardFactory, fakeMatchResultsRepository, fakeLeagueRecordsRepository);

            playerRecordOne = A.Fake<PlayerRecord>();
            playerRecordTwo = A.Fake<PlayerRecord>();

            playerOneWin = SetUpMatchResult(playerRecordOne, playerRecordTwo, laterDate);
            playerTwoWin = SetUpMatchResult(playerRecordTwo, playerRecordOne, laterDate);
        }

        [Test]
        public void Latest_League_Results_returns_json_results()
        {
            //Given

            var matchResults = new List<MatchResult> { playerOneWin, playerTwoWin, playerTwoWin };
            var expectedLeaderboard = new List<PlayerStats>
                                      {
                                          new PlayerStats
                                          {
                                              Id = playerRecordOne.Id,
                                              Name = playerRecordOne.Name,
                                              Wins = 1,
                                              Losses = 2
                                          },
                                          new PlayerStats
                                          {
                                              Id = playerRecordTwo.Id,
                                              Name = playerRecordTwo.Name,
                                              Wins = 2,
                                              Losses = 1
                                          }
                                      };
            A.CallTo(() => fakeLeagueRecordsRepository.GetLatestLeagueTime()).Returns(middleDate);
            A.CallTo(() => fakeMatchResultsRepository.GetAll()).Returns(matchResults);
            A.CallTo(() => fakeLeaderboardFactory.GenerateLeaderboard(A<List<MatchResult>>.That.IsSameSequenceAs(matchResults))).Returns(expectedLeaderboard);

            //When
            var result = controller.LatestLeagueResults();
            //Then
            Assert.That(result, IsMVC.Json(expectedLeaderboard));
        }

        [Test]
        public void UpdatedPlayers_only_contains_players_with_LastUpdated_later_than_most_recent_league_start()
        {
            // Given
            playerRecordOne.LastUpdated = earlierDate;
            playerRecordTwo.LastUpdated = laterDate;

            var fakeBattleshipsBot = A.Fake<IBattleshipsBot>();

            var playerOne = new BattleshipsPlayer(fakeBattleshipsBot, playerRecordOne);
            var playerTwo = new BattleshipsPlayer(fakeBattleshipsBot, playerRecordTwo);

            A.CallTo(() => fakePlayerRecordsRepository.GetAll()).Returns(new List<PlayerRecord> { playerRecordOne, playerRecordTwo });

            A.CallTo(() => fakeBattleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecord(playerRecordOne)).Returns(playerOne);
            A.CallTo(() => fakeBattleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecord(playerRecordTwo)).Returns(playerTwo);
            A.CallTo(() => fakeLeagueRecordsRepository.GetLatestLeagueTime()).Returns(middleDate);

            // When
            controller.RunLeague();

            // Then
            A.CallTo(() => fakeLeagueRunner.GetLeagueResults(A<List<IBattleshipsPlayer>>.That.Contains(playerOne), A<List<IBattleshipsPlayer>>.That.Contains(playerTwo), NumberOfRounds)).MustHaveHappened();
            A.CallTo(() => fakeLeagueRunner.GetLeagueResults(A<List<IBattleshipsPlayer>>.That.Contains(playerTwo), A<List<IBattleshipsPlayer>>.That.Not.Contains(playerOne), NumberOfRounds)).MustHaveHappened();
        }

        private MatchResult SetUpMatchResult(PlayerRecord winner, PlayerRecord loser, DateTime timePlayed)
        {
            var matchResult = A.Fake<MatchResult>();
            matchResult.Winner = winner;
            matchResult.Loser = loser;
            matchResult.WinnerWins = 2;
            matchResult.LoserWins = 1;
            matchResult.TimePlayed = timePlayed;

            return matchResult;
        }
    }
}