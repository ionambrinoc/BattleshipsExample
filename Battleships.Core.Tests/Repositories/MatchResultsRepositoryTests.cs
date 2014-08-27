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
        private readonly DateTime earlierDate = new DateTime(2001, 1, 1);
        private readonly DateTime laterDate = new DateTime(2002, 1, 1);
        private MatchResultsRepository repo;
        private PlayerRecord player1;
        private PlayerRecord player2;
        private PlayerRecord player3;
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
        }

        [Test]
        public void Will_overwrite_result_if_players_already_played_each_other()
        {
            // Given
            repo.UpdateResults(new List<MatchResult> { CreateMatchResult(player1, player2, earlierDate) });
            repo.SaveContext();

            // When
            repo.UpdateResults(new List<MatchResult> { CreateMatchResult(player1, player2, laterDate) });
            repo.SaveContext();

            var listOfResults = repo.GetAll().Where(match => match.Winner == player1 && match.Loser == player2).ToArray();

            // Then
            listOfResults.Count().Should().Be(1);
            listOfResults[0].TimePlayed.Should().Be(laterDate);
        }

        [Test]
        public void Will_add_result_if_players_have_not_played_each_other()
        {
            // Given
            repo.UpdateResults(new List<MatchResult> { CreateMatchResult(player1, player2, earlierDate) });
            repo.SaveContext();

            // When
            repo.UpdateResults(new List<MatchResult> { CreateMatchResult(player1, player3, laterDate) });
            repo.SaveContext();

            // Then
            repo.GetAll().Count().Should().Be(2);
            repo.GetAll().Count(match => match.Winner == player1 && match.Loser == player3).Should().Be(1);
        }

        [Test]
        public void Displays_results_between_all_players()
        {
            // Given
            repo.UpdateResults(new List<MatchResult>
                               {
                                   CreateMatchResult(player1, player2, earlierDate),
                                   CreateMatchResult(player1, player3, earlierDate),
                                   CreateMatchResult(player2, player3, earlierDate)
                               });
            repo.SaveContext();

            // When
            var listOfTwoPlayerIds = new List<int> { player1.Id, player2.Id };
            var listOfThreePlayerIds = new List<int> { player1.Id, player2.Id, player3.Id };
            var matchResults1 = repo.GetAllMatchResults(listOfTwoPlayerIds);
            var matchResults2 = repo.GetAllMatchResults(listOfThreePlayerIds);

            // Then
            matchResults1.Count().Should().Be(1);
            matchResults2.Count().Should().Be(3);
        }

        [Test]
        public void Inherited_method_Add_throws_exception()
        {
            // When
            Action action = () => repo.Add(CreateMatchResult(player1, player2, earlierDate));

            // Then
            action.ShouldThrow<InvalidOperationException>().WithMessage(
                "The 'Add' method for MatchResultsRepository can lead to duplicate entries and should not be used. Use 'UpdateResults' instead.");
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