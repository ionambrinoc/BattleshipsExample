namespace Battleships.Runner.Tests.Runners
{
    using Battleships.Core.Models;
    using Battleships.Player;
    using Battleships.Runner.Runners;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class LeagueRunnerTests
    {
        private const int NumberOfRounds = 3;
        private LeagueRunner runner;
        private IMatchRunner fakeMatchRunner;
        private IBattleshipsPlayer playerOne;
        private IBattleshipsPlayer playerTwo;
        private IBattleshipsPlayer playerThree;
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
            playerThree = A.Fake<IBattleshipsPlayer>();
            players = new List<IBattleshipsPlayer> { playerOne, playerTwo, playerThree };

            playerOneWin = A.Fake<MatchResult>();
            playerTwoWin = A.Fake<MatchResult>();
        }

        [Test]
        public void List_returned_with_one_win_each()
        {
            // Given
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerTwo, NumberOfRounds)).Returns(playerOneWin);
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerThree, NumberOfRounds)).Returns(playerOneWin);
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerTwo, playerThree, NumberOfRounds)).Returns(playerTwoWin);

            // When
            var results = runner.GetLeagueResults(players, NumberOfRounds);

            // Then
            results.ShouldBeEquivalentTo(new List<MatchResult> { playerOneWin, playerOneWin, playerTwoWin });
        }

        [Test]
        public void Players_dont_play_themselves()
        {
            // When
            runner.GetLeagueResults(players, NumberOfRounds);

            // Then
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerOne, NumberOfRounds)).MustNotHaveHappened();
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerTwo, playerTwo, NumberOfRounds)).MustNotHaveHappened();
        }

        [Test]
        public void Players_dont_play_each_other_twice()
        {
            // When
            runner.GetLeagueResults(players, NumberOfRounds);

            // Then
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerTwo, NumberOfRounds)).MustHaveHappened();
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerTwo, playerOne, NumberOfRounds)).MustNotHaveHappened();
        }
    }
}
