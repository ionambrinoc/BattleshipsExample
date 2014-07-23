namespace Battleships.Web.Tests.Factories
{
    using Battleships.Runner.Models;
    using Battleships.Web.Factories;
    using Battleships.Web.Models.League;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class LeaderboardFactoryTests
    {
        private PlayerRecord playerOne;
        private PlayerRecord playerTwo;
        private MatchResult matchWhenPlayerOneWins;
        private MatchResult matchWhenPlayerTwoWins;
        private List<MatchResult> matchResults;
        private LeaderboardFactory leaderboardFactory;
        private RoundStats playerOneWinRoundStats;
        private RoundStats playerTwoWinRoundStats;
        private RoundStats playerOneLoseRoundStats;
        private RoundStats playerTwoLoseRoundStats;

        [SetUp]
        public void SetUp()
        {
            playerOne = A.Fake<PlayerRecord>();
            playerTwo = A.Fake<PlayerRecord>();
            playerOne.Id = 1;
            playerTwo.Id = 2;

            matchWhenPlayerOneWins = A.Fake<MatchResult>();
            matchWhenPlayerTwoWins = A.Fake<MatchResult>();

            matchWhenPlayerOneWins.Winner = playerOne;
            matchWhenPlayerOneWins.Loser = playerTwo;
            matchWhenPlayerTwoWins.Winner = playerTwo;
            matchWhenPlayerTwoWins.Loser = playerOne;

            playerOneWinRoundStats = new RoundStats { Losses = 1, Wins = 2, OpponentName = playerTwo.Name };
            playerTwoWinRoundStats = new RoundStats { Losses = 1, Wins = 2, OpponentName = playerOne.Name };
            playerOneLoseRoundStats = new RoundStats { Losses = 2, Wins = 1, OpponentName = playerTwo.Name };
            playerTwoLoseRoundStats = new RoundStats { Losses = 2, Wins = 1, OpponentName = playerOne.Name };

            leaderboardFactory = new LeaderboardFactory();
        }

        [Test]
        public void Leaderboard_returned_with_player_with_most_wins_on_top()
        {
            // Given
            matchResults = new List<MatchResult> { matchWhenPlayerOneWins, matchWhenPlayerOneWins, matchWhenPlayerTwoWins };
            WinnerRoundWinsWhenPlayerOneWins(2);
            WinnerRoundWinsWhenPlayerTwoWins(2);

            var expectedResults = new List<PlayerStats>
                                  {
                                      new PlayerStats
                                      {
                                          Id = playerOne.Id,
                                          Name = playerOne.Name,
                                          Wins = 2,
                                          Losses = 1,
                                          RoundStats = new List<RoundStats> { playerOneWinRoundStats, playerOneWinRoundStats, playerOneLoseRoundStats }
                                      },
                                      new PlayerStats
                                      {
                                          Id = playerTwo.Id,
                                          Name = playerTwo.Name,
                                          Wins = 1,
                                          Losses = 2,
                                          RoundStats = new List<RoundStats> { playerTwoWinRoundStats, playerTwoLoseRoundStats, playerTwoLoseRoundStats }
                                      }
                                  };

            // When
            var results = leaderboardFactory.GenerateLeaderboard(matchResults);

            // Then
            results.ShouldBeEquivalentTo(expectedResults);
        }

        [Test]
        public void Sorts_by_round_wins_when_total_wins_the_same()
        {
            // Given
            matchResults = new List<MatchResult> { matchWhenPlayerOneWins, matchWhenPlayerTwoWins };
            WinnerRoundWinsWhenPlayerOneWins(2);
            WinnerRoundWinsWhenPlayerTwoWins(3);

            // When
            var results = leaderboardFactory.GenerateLeaderboard(matchResults);

            // Then
            results[0].Wins.Should().Be(results[1].Wins);
            results.Should().BeInDescendingOrder(x => x.TotalRoundWins);
        }

        private void WinnerRoundWinsWhenPlayerOneWins(int wins)
        {
            matchWhenPlayerOneWins.WinnerWins = wins;
            matchWhenPlayerOneWins.LoserWins = 3 - wins;
        }

        private void WinnerRoundWinsWhenPlayerTwoWins(int wins)
        {
            matchWhenPlayerTwoWins.WinnerWins = wins;
            matchWhenPlayerTwoWins.LoserWins = 3 - wins;
        }
    }
}