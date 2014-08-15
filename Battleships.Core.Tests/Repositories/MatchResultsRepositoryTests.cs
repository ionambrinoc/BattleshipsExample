namespace Battleships.Core.Tests.Repositories
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Core.Tests.TestHelpers;
    using Battleships.Core.Tests.TestHelpers.Attributes;
    using Battleships.Core.Tests.TestHelpers.Database;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    [RequiresDatabase]
    public class MatchResultsRepositoryTests
    {
        private readonly DateTime date1 = new DateTime(2001, 1, 1);
        private readonly DateTime date2 = new DateTime(2002, 1, 1);
        private MatchResultsRepository repo;
        private PlayerRecord player1;
        private PlayerRecord player2;
        private PlayerRecord player3;
        private MatchResult matchResult1;
        private MatchResult matchResult2;
        private string userId;

        [SetUp]
        public void SetUp()
        {
            var testUserManager = new TestUserManager(TestDb.ConnectionString);
            userId = testUserManager.CreateUserAndGetId("user1");
            repo = new MatchResultsRepository(new TestBattleshipsContext(TestDb.ConnectionString));
            player1 = CreatePlayer("Player1");
            player2 = CreatePlayer("Player2");
            player3 = CreatePlayer("Player3");
            matchResult1 = CreateMatchResult(player1, player2, date1);
            matchResult2 = CreateMatchResult(player1, player3, date2);
            repo.Add(matchResult1);
            repo.Add(matchResult2);
            repo.SaveContext();
        }

        [Test]
        public void Can_get_most_recent_time()
        {
            // When
            var result = repo.GetMostRecentMatchTime();

            // Then
            result.Should().Be(date2);
        }

        [Test]
        public void Will_overwrite_result_if_players_already_played_each_other()
        {
            // Given
            var newMatch = matchResult1;
            newMatch.TimePlayed = date2;

            // When
            repo.UpdateResults(new List<MatchResult> { newMatch });
            repo.SaveContext();

            // Then
            repo.GetAll().Count().Should().Be(2);
            repo.GetAll().Count(match => match.TimePlayed == date2 && match.Winner == player1 && match.Loser == player2).Should().Be(1);
            repo.GetAll().Count(match => match == newMatch).Should().Be(1);
        }

        [Test]
        public void Will_add_result_if_players_have_not_played_each_other()
        {
            // Given
            var newMatch = CreateMatchResult(player2, player3, DateTime.Now);

            // When
            repo.UpdateResults(new List<MatchResult> { newMatch });
            repo.SaveContext();

            // Then
            repo.GetAll().Count().Should().Be(3);
            repo.GetAll().Count(match => match.Winner == player2 && match.Loser == player3).Should().Be(1);
        }

        private PlayerRecord CreatePlayer(string name)
        {
            return new PlayerRecord { Name = name, UserId = userId, LastUpdated = DateTime.Now };
        }

        private MatchResult CreateMatchResult(PlayerRecord winner, PlayerRecord loser, DateTime timePlayed)
        {
            return new MatchResult { Winner = winner, Loser = loser, TimePlayed = timePlayed };
        }
    }
}