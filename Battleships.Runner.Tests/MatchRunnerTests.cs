namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    internal class MatchRunnerTests
    {
        private IMatchRunner matchRunner;
        private IHeadToHeadRunner headToHeadRunner;
        private IPlayerRecordsRepository playerRecordsRepository;
        private IMatchScoreBoardFactory matchScoreBoardFactory;
        private IBattleshipsPlayer playerOne;
        private IBattleshipsPlayer playerTwo;
        private PlayerRecord winnerPlayerRecord;
        private PlayerRecord loserPlayerRecord;
        private IMatchScoreBoard matchScoreBoard;

        [SetUp]
        public void SetUp()
        {
            headToHeadRunner = A.Fake<IHeadToHeadRunner>();

            playerOne = A.Fake<IBattleshipsPlayer>();
            playerTwo = A.Fake<IBattleshipsPlayer>();

            playerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            winnerPlayerRecord = A.Fake<PlayerRecord>();
            loserPlayerRecord = A.Fake<PlayerRecord>();

            matchScoreBoardFactory = A.Fake<IMatchScoreBoardFactory>();
            matchScoreBoard = A.Fake<IMatchScoreBoard>();
            A.CallTo(() => matchScoreBoardFactory.GetMatchScoreBoard(playerOne, playerTwo)).Returns(matchScoreBoard);

            A.CallTo(() => matchScoreBoard.PlayerOneCounter).Returns(1);
            A.CallTo(() => matchScoreBoard.PlayerTwoCounter).Returns(0);

            matchRunner = new MatchRunner(headToHeadRunner, playerRecordsRepository, matchScoreBoardFactory);
        }

        [TestCaseSource("Games")]
        public void Winner_has_winner_win_count_and_wins_and_same_for_loser(int expectedWinner, int expectedLoser)
        {
            // Given
            SetPlayerWinner(Player(expectedWinner));
            SetPlayerLoser(Player(expectedLoser));

            SetPlayerWinnerCounter(69);
            SetPlayerLoserCounter(31);

            // When
            var result = GetMatchResult();

            // Then
            result.Winner.ShouldBeEquivalentTo(winnerPlayerRecord);
            result.Loser.ShouldBeEquivalentTo(loserPlayerRecord);
            result.WinnerWins.Should().Be(69);
            result.LoserWins.Should().Be(31);
        }

        [Test]
        public void Number_of_winners_is_the_same_as_number_of_rounds()
        {
            // When
            GetMatchResult(69);

            // Then
            A.CallTo(() => headToHeadRunner.FindWinner(A<IBattleshipsPlayer>._, A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(69));
        }

        [Test]
        public void Player_swaps_after_each_round()
        {
            // When
            GetMatchResult(3);

            // Then
            A.CallTo(() => headToHeadRunner.FindWinner(playerOne, playerTwo)).MustHaveHappened(Repeated.Exactly.Times(2));
            A.CallTo(() => headToHeadRunner.FindWinner(playerTwo, playerOne)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Draw_plays_one_more_round()
        {
            // Given
            SetMatchAsADraw();

            // When
            GetMatchResult(110);

            // Then
            A.CallTo(() => headToHeadRunner.FindWinner(A<IBattleshipsPlayer>._, A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(111));
        }

        private static IEnumerable<int[]> Games()
        {
            yield return new[] { 1, 2 };
            yield return new[] { 2, 1 };
        }

        private void SetMatchAsADraw()
        {
            A.CallTo(() => matchScoreBoard.PlayerOneCounter).Returns(0);
        }

        private void SetPlayerLoserCounter(int playerCount)
        {
            A.CallTo(() => matchScoreBoard.GetLoserCounter()).Returns(playerCount);
        }

        private void SetPlayerWinnerCounter(int playerCount)
        {
            A.CallTo(() => matchScoreBoard.GetWinnerCounter()).Returns(playerCount);
        }

        private void SetPlayerWinner(IBattleshipsPlayer player)
        {
            A.CallTo(() => matchScoreBoard.GetWinner()).Returns(player);
            A.CallTo(() => playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(player)).Returns(winnerPlayerRecord);
        }

        private void SetPlayerLoser(IBattleshipsPlayer player)
        {
            A.CallTo(() => matchScoreBoard.GetLoser()).Returns(player);
            A.CallTo(() => playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(player)).Returns(loserPlayerRecord);
        }

        private MatchResult GetMatchResult(int rounds = 100)
        {
            return matchRunner.GetMatchResult(playerOne, playerTwo, rounds);
        }

        private IBattleshipsPlayer Player(int number)
        {
            switch (number)
            {
                case 1:
                    return playerOne;
                case 2:
                    return playerTwo;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}