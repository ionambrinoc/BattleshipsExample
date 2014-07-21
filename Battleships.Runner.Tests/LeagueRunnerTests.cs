namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;

    internal class LeagueRunnerTests
    {
        private const int DefaultNumberOfRounds = 1;
        private LeagueRunner runner;
        private IMatchRunner fakeMatchRunner;
        private IBattleshipsPlayer playerOne;
        private IBattleshipsPlayer playerTwo;
        private List<IBattleshipsPlayer> players;
        private MatchResult playerOneWin;
        private MatchResult playerTwoWin;

        [SetUp]
        public void SetUp()
        {
            fakeMatchRunner = A.Fake<IMatchRunner>();
            runner = new LeagueRunner(fakeMatchRunner);

            playerOne = A.Fake<IBattleshipsPlayer>();
            playerTwo = A.Fake<IBattleshipsPlayer>();
            players = new List<IBattleshipsPlayer> { playerOne, playerTwo };

            playerOneWin = A.Fake<MatchResult>();
            playerTwoWin = A.Fake<MatchResult>();
        }

        [Test]
        public void List_returned_with_one_win_each()
        {
            // Given
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerTwo, DefaultNumberOfRounds)).Returns(playerOneWin);
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerTwo, playerOne, DefaultNumberOfRounds)).Returns(playerTwoWin);

            // When
            var results = runner.GetLeagueResults(players);

            // Then
            results.ShouldBeEquivalentTo(new List<MatchResult> { playerOneWin, playerTwoWin });
        }
    }
}