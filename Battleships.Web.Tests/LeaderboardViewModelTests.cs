namespace Battleships.Web.Tests
{
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using Battleships.Web.Models.League;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;

    internal class LeaderboardViewModelTests
    {
        private PlayerRecord playerOne;
        private PlayerRecord playerTwo;
        private MatchResult matchWhenPlayerOneWins;
        private MatchResult matchWhenPlayerTwoWins;
        private List<MatchResult> matchResults;
        private LeaderboardViewModel leaderboardViewModel;

        [SetUp]
        public void SetUp()
        {
            playerOne = A.Fake<PlayerRecord>();
            playerTwo = A.Fake<PlayerRecord>();

            matchWhenPlayerOneWins = A.Fake<MatchResult>();
            matchWhenPlayerTwoWins = A.Fake<MatchResult>();

            A.CallTo(() => matchWhenPlayerOneWins.Winner).Returns(playerOne);
            A.CallTo(() => matchWhenPlayerOneWins.Loser).Returns(playerTwo);
            A.CallTo(() => matchWhenPlayerTwoWins.Winner).Returns(playerTwo);
            A.CallTo(() => matchWhenPlayerTwoWins.Loser).Returns(playerOne);

            leaderboardViewModel = new LeaderboardViewModel();
        }

        [Test]
        public void Leaderboard_returned_with_player_one_on_top()
        {
            // Given
            matchResults = new List<MatchResult> { matchWhenPlayerOneWins, matchWhenPlayerOneWins, matchWhenPlayerTwoWins };
            WinnerRoundWinsWhenPlayerOneWins(2);
            WinnerRoundWinsWhenPlayerTwoWins(2);

            var expectedResults = new List<PlayerStats>
                                  {
                                      new PlayerStats(playerOne, 2, 5, 1),
                                      new PlayerStats(playerTwo, 1, 4, 2)
                                  };

            // When
            var results = leaderboardViewModel.GenerateLeaderboard(matchResults);

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
            var results = leaderboardViewModel.GenerateLeaderboard(matchResults);
            var totalWinsTheSame = results[0].Wins == results[1].Wins;

            // Then
            totalWinsTheSame.Should().BeTrue();
            results.Should().BeInDescendingOrder(x => x.RoundWins);
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