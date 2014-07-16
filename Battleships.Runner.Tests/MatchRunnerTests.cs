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

        [SetUp]
        public void SetUp()
        {
            headToHeadRunner = A.Fake<IHeadToHeadRunner>();
            playerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            matchScoreBoardFactory = A.Fake<IMatchScoreBoardFactory>();
            playerOne = A.Fake<IBattleshipsPlayer>();
            playerTwo = A.Fake<IBattleshipsPlayer>();
            winnerPlayerRecord = A.Fake<PlayerRecord>();
            loserPlayerRecord = A.Fake<PlayerRecord>();
            matchRunner = new MatchRunner(headToHeadRunner, playerRecordsRepository, matchScoreBoardFactory);
        }

        [TestCaseSource("Games")]
        public void Get_match_result_returns_correct_match_result(int expectedWinner, int expectedLoser)
        {
            // Given
            var matchHelper = SetUpMatchHelper();
            SetPlayerWinner(Player(expectedWinner), matchHelper);
            SetPlayerWinnerCounter(69, matchHelper);
            SetPlayerLoser(Player(expectedLoser), matchHelper);
            SetPlayerLoserCounter(31, matchHelper);

            // When
            var result = GetMatchResult();

            // Then
            result.Winner.ShouldBeEquivalentTo(winnerPlayerRecord);
            result.Loser.ShouldBeEquivalentTo(loserPlayerRecord);
            result.WinnerWins.Should().Be(69);
            result.LoserWins.Should().Be(31);
        }

        [Test]
        public void Runs_find_winner_correct_number_of_times()
        {
            // Given
            var matchHelper = SetUpMatchHelper();
            SetPlayerWinnerCounter(50, matchHelper);
            SetPlayerLoserCounter(19, matchHelper);
            A.CallTo(() => matchHelper.PlayerOneCounter).Returns(1);

            // When
            GetMatchResult(69);

            // Then
            A.CallTo(() => matchHelper.IncrementWinnerCounter(A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(69));
        }

        [Test]
        public void Draw_plays_one_more_round()
        {
            // Given
            var matchHelper = SetUpMatchHelper();
            SetPlayerWinnerCounter(50, matchHelper);
            SetPlayerLoserCounter(19, matchHelper);

            // When
            GetMatchResult(69);

            // Then
            A.CallTo(() => matchHelper.IncrementWinnerCounter(A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(70));
        }

        private static IEnumerable<int[]> Games()
        {
            yield return new[] { 1, 2 };
            yield return new[] { 2, 1 };
        }

        private void SetPlayerLoserCounter(int playerCount, IMatchScoreBoard matchScoreBoard)
        {
            A.CallTo(() => matchScoreBoard.GetLoserCounter()).Returns(playerCount);
        }

        private void SetPlayerWinnerCounter(int playerCount, IMatchScoreBoard matchScoreBoard)
        {
            A.CallTo(() => matchScoreBoard.GetWinnerCounter()).Returns(playerCount);
        }

        private IMatchScoreBoard SetUpMatchHelper()
        {
            var matchHelper = A.Fake<IMatchScoreBoard>();
            A.CallTo(() => matchScoreBoardFactory.GetMatchScoreBoard(playerOne, playerTwo)).Returns(matchHelper);
            return matchHelper;
        }

        private void SetPlayerWinner(IBattleshipsPlayer player, IMatchScoreBoard matchScoreBoard)
        {
            A.CallTo(() => matchScoreBoard.GetWinner()).Returns(player);
            A.CallTo(() => playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(player)).Returns(winnerPlayerRecord);
        }

        private void SetPlayerLoser(IBattleshipsPlayer player, IMatchScoreBoard matchScoreBoard)
        {
            A.CallTo(() => matchScoreBoard.GetLoser()).Returns(player);
            A.CallTo(() => playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(player)).Returns(loserPlayerRecord);
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

        private MatchResult GetMatchResult(int rounds = 100)
        {
            return matchRunner.GetMatchResult(playerOne, playerTwo, rounds);
        }
    }
}