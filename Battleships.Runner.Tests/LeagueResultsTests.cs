namespace Battleships.Runner.Tests
{
    using Battleships.Runner.Models;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;

    internal class LeagueResultsTests
    {
        private PlayerRecord playerOne;
        private PlayerRecord playerTwo;
        private MatchResult matchWhenPlayerOneWins;
        private MatchResult matchWhenPlayerTwoWins;
        private List<MatchResult> matchResults;
        private LeagueResults leagueResults;

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

            leagueResults = new LeagueResults();
        }

        [Test]
        public void Leaderboard_returned_with_player_one_on_top()
        {
            // Given
            matchResults = new List<MatchResult> { matchWhenPlayerOneWins, matchWhenPlayerOneWins, matchWhenPlayerTwoWins };
            matchWhenPlayerOneWins.WinnerWins = 2;
            matchWhenPlayerOneWins.LoserWins = 1;
            matchWhenPlayerTwoWins.WinnerWins = 2;
            matchWhenPlayerTwoWins.LoserWins = 1;

            var expectedResults = new List<KeyValuePair<PlayerRecord, PlayerStats>>
                                  {
                                      new KeyValuePair<PlayerRecord, PlayerStats>(playerOne, new PlayerStats(2, 5, 1)),
                                      new KeyValuePair<PlayerRecord, PlayerStats>(playerTwo, new PlayerStats(1, 4, 2))
                                  };

            // When
            var results = leagueResults.GenerateLeaderboard(matchResults);

            // Then
            results.ShouldBeEquivalentTo(expectedResults);
        }

        [Test]
        public void Sorts_by_round_wins_when_total_wins_the_same()
        {

            // Given
            matchResults = new List<MatchResult> { matchWhenPlayerOneWins, matchWhenPlayerTwoWins };
            matchWhenPlayerOneWins.WinnerWins = 2;
            matchWhenPlayerOneWins.LoserWins = 1;
            matchWhenPlayerTwoWins.WinnerWins = 3;
            matchWhenPlayerTwoWins.LoserWins = 0;

            // When
            var results = leagueResults.GenerateLeaderboard(matchResults);
            var totalWinsTheSame = results[0].Value.Wins == results[1].Value.Wins;

            // Then
            totalWinsTheSame.Should().BeTrue();
            results.Should().BeInDescendingOrder(x => x.Value.RoundWins);
        }
    }
}