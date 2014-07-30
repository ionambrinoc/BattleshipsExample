namespace Battleships.Runner.Tests
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Factories;
    using Battleships.Runner.Models;
    using Battleships.Runner.Services;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    internal class MatchRunnerTests
    {
        private const int NumberOfRounds = 69;
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

            winnerPlayerRecord.Id = 1;
            loserPlayerRecord.Id = 2;

            matchScoreBoardFactory = A.Fake<IMatchScoreBoardFactory>();
            matchScoreBoard = A.Fake<IMatchScoreBoard>();
            A.CallTo(() => matchScoreBoardFactory.GetMatchScoreBoard(playerOne, playerTwo)).Returns(matchScoreBoard);

            A.CallTo(() => matchScoreBoard.IsDraw()).Returns(false);

            matchRunner = new MatchRunner(headToHeadRunner, playerRecordsRepository, matchScoreBoardFactory);
        }

        [TestCaseSource("Games")]
        public void Winner_has_winner_win_count_and_wins_and_same_for_loser(int expectedWinner, int expectedLoser)
        {
            // Given
            SetPlayerWinner(Player(expectedWinner));
            SetPlayerLoser(Player(expectedLoser));

            SetPlayerWinnerWins(69);
            SetPlayerLoserWins(31);

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
            GetMatchResult(NumberOfRounds);

            // Then
            A.CallTo(() => headToHeadRunner.FindWinner(A<IBattleshipsPlayer>._, A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(NumberOfRounds));
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
            A.CallTo(() => matchScoreBoard.IsDraw()).Returns(true);

            // When
            GetMatchResult(NumberOfRounds);

            // Then
            A.CallTo(() => headToHeadRunner.FindWinner(A<IBattleshipsPlayer>._, A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(NumberOfRounds + 1));
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<int[]> Games()
        {
            yield return new[] { 1, 2 };
            yield return new[] { 2, 1 };
        }

        private void SetPlayerLoserWins(int playerCount)
        {
            A.CallTo(() => matchScoreBoard.GetLoserWins()).Returns(playerCount);
        }

        private void SetPlayerWinnerWins(int playerCount)
        {
            A.CallTo(() => matchScoreBoard.GetWinnerWins()).Returns(playerCount);
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
