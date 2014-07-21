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
        private MatchResult playerOneWin;
        private MatchResult playerTwoWin;
        private List<MatchResult> matchResults;
        private LeagueResults leagueResults;

        [SetUp]
        public void SetUp()
        {
            playerOne = A.Fake<PlayerRecord>();
            playerTwo = A.Fake<PlayerRecord>();

            playerOneWin = A.Fake<MatchResult>();
            playerTwoWin = A.Fake<MatchResult>();
            matchResults = new List<MatchResult> { playerOneWin, playerOneWin, playerTwoWin };

            leagueResults = new LeagueResults();
        }

        [Test]
        public void Leaderboard_returned_with_player_one_on_top()
        {
            // Given
            A.CallTo(() => playerOneWin.Winner).Returns(playerOne);
            A.CallTo(() => playerOneWin.Loser).Returns(playerTwo);
            A.CallTo(() => playerTwoWin.Winner).Returns(playerTwo);
            A.CallTo(() => playerTwoWin.Loser).Returns(playerOne);
            var expectedResults = new List<KeyValuePair<PlayerRecord, PlayerStats>>
                                  {
                                      new KeyValuePair<PlayerRecord, PlayerStats>(playerOne, new PlayerStats(2, 1)),
                                      new KeyValuePair<PlayerRecord, PlayerStats>(playerTwo, new PlayerStats(1, 2))
                                  };

            // When
            var results = leagueResults.GenerateLeaderboard(matchResults);

            // Then
            results.ShouldBeEquivalentTo(expectedResults);
        }
    }
}