namespace Battleships.Runner.Tests.Runners
{
    using Battleships.Core.Models;
    using Battleships.Player;
    using Battleships.Runner.Runners;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class LeagueRunnerTests
    {
        private const int NumberOfRounds = 3;
        private LeagueRunner runner;
        private IMatchRunner fakeMatchRunner;
        private IBattleshipsPlayer playerOne;
        private IBattleshipsPlayer playerTwo;
        private IBattleshipsPlayer playerThree;
        private List<IBattleshipsPlayer> players;
        private List<IBattleshipsPlayer> recentlyUpdatedPlayers;
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
            recentlyUpdatedPlayers = new List<IBattleshipsPlayer> { playerOne, playerTwo, playerThree };

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
            var results = runner.GetLeagueResults(players, recentlyUpdatedPlayers, NumberOfRounds);

            // Then
            results.ShouldBeEquivalentTo(new List<MatchResult> { playerOneWin, playerOneWin, playerTwoWin });
        }

        [Test]
        public void Players_dont_play_themselves()
        {
            // When
            runner.GetLeagueResults(players, recentlyUpdatedPlayers, NumberOfRounds);

            // Then
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerOne, NumberOfRounds)).MustNotHaveHappened();
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerTwo, playerTwo, NumberOfRounds)).MustNotHaveHappened();
        }

        [Test]
        public void Players_do_not_play_each_other_twice()
        {
            // When
            runner.GetLeagueResults(players, recentlyUpdatedPlayers, NumberOfRounds);

            // Then
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerTwo, NumberOfRounds)).MustHaveHappened();
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerTwo, playerOne, NumberOfRounds)).MustNotHaveHappened();
        }

        [Test]
        public void Old_players_do_not_play_each_other()
        {
            // Given
            var emptyPlayerList = new List<IBattleshipsPlayer>();

            // When
            runner.GetLeagueResults(players, emptyPlayerList, NumberOfRounds);

            // Then
            A.CallTo(() => fakeMatchRunner.GetMatchResult(A<IBattleshipsPlayer>._, A<IBattleshipsPlayer>._, NumberOfRounds)).MustNotHaveHappened();
        }

        [Test]
        public void New_players_play_exactly_one_match_against_every_other_player()
        {
            // Given
            var onePlayerList = new List<IBattleshipsPlayer> { playerOne };

            // When
            runner.GetLeagueResults(players, onePlayerList, NumberOfRounds);

            // Then
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerTwo, NumberOfRounds)).MustHaveHappened();
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerOne, playerThree, NumberOfRounds)).MustHaveHappened();
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerTwo, playerThree, NumberOfRounds)).MustNotHaveHappened();
            A.CallTo(() => fakeMatchRunner.GetMatchResult(playerThree, playerTwo, NumberOfRounds)).MustNotHaveHappened();
        }
    }
}