namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class HeadToHeadRunnerTests
    {
        private HeadToHeadRunner runner;
        private IBattleshipsPlayer playerOne;
        private IBattleshipsPlayer playerTwo;
        private IShipsPlacementFactory shipsPlacementFactory;
        private IPlayerRecordsRepository playerRecordsRepository;
        private IMatchHelperFactory matchHelperFactory;
        private PlayerRecord winnerPlayerRecord;
        private PlayerRecord loserPlayerRecord;

        [SetUp]
        public void SetUp()
        {
            winnerPlayerRecord = A.Fake<PlayerRecord>();
            loserPlayerRecord = A.Fake<PlayerRecord>();
            playerOne = A.Fake<IBattleshipsPlayer>();
            playerTwo = A.Fake<IBattleshipsPlayer>();
            shipsPlacementFactory = A.Fake<IShipsPlacementFactory>();
            playerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            matchHelperFactory = A.Fake<IMatchHelperFactory>();
            runner = new HeadToHeadRunner(shipsPlacementFactory, matchHelperFactory, playerRecordsRepository);
        }

        [Test]
        public void Two_consecutive_games_do_not_interfere()
        {
            PlayerIsValid(playerOne);
            PlayerIsValid(playerTwo);

            ShipsNotAllHit(playerOne);
            ShipsAllHit(playerTwo);
            var firstWinner = FindWinner();
            firstWinner.Should().Be(playerOne);

            ShipsNotAllHit(playerTwo);
            ShipsAllHit(playerOne);
            var secondWinner = FindWinner();
            secondWinner.Should().Be(playerTwo);
        }

        [TestCaseSource("Games")]
        public void Player_loses_if_ship_positions_invalid(int expectedWinner, int expectedLoser)
        {
            // Given
            PlayerIsValid(Player(expectedWinner));
            PlayerIsInvalid(Player(expectedLoser));

            // When
            var winner = FindWinner();

            // Then
            Assert.That(winner, IsPlayer(expectedWinner));
        }

        [Test]
        public void Player_two_wins_if_both_invalid()
        {
            // Given
            PlayerIsInvalid(playerOne);
            PlayerIsInvalid(playerTwo);

            // When
            var winner = FindWinner();

            // Then
            winner.Should().Be(playerTwo);
        }

        [TestCaseSource("Games")]
        public void Player_with_all_ships_hit_loses(int expectedWinner, int expectedLoser)
        {
            // Given
            PlayerIsValid(Player(expectedWinner));
            PlayerIsValid(Player(expectedLoser));
            ShipsNotAllHit(Player(expectedWinner));
            ShipsAllHit(Player(expectedLoser));

            // When
            var winner = FindWinner();

            // Then
            winner.Should().Be(Player(expectedWinner));
        }

        [Test]
        public void Player_loses_on_timeout() {}

        [TestCaseSource("Games")]
        public void Get_match_result_returns_correct_match_result(int expectedWinner, int expectedLoser)
        {
            // Given
            var matchHelper = SetUpMatchHelper(Player(expectedWinner), Player(expectedLoser));
            SetPlayerWinner(Player(expectedWinner), matchHelper);
            SetPlayerWinnerCounter(69, matchHelper);
            SetPlayerLoser(Player(expectedLoser), matchHelper);
            SetPlayerLoserCounter(31, matchHelper);

            // When
            var result = runner.GetMatchResult(Player(expectedWinner), Player(expectedLoser));

            // Then
            result.Winner.ShouldBeEquivalentTo(winnerPlayerRecord);
            result.Loser.ShouldBeEquivalentTo(loserPlayerRecord);
            result.WinnerWins.Should().Be(69);
            result.LoserWins.Should().Be(31);
        }

        private void SetPlayerLoserCounter(int playerCount, IMatchHelper matchHelper)
        {
            A.CallTo(() => matchHelper.GetLoserCounter()).Returns(playerCount);
        }

        private void SetPlayerWinnerCounter(int playerCount, IMatchHelper matchHelper)
        {
            A.CallTo(() => matchHelper.GetWinnerCounter()).Returns(playerCount);
        }

        private IMatchHelper SetUpMatchHelper(IBattleshipsPlayer winner, IBattleshipsPlayer loser)
        {
            var matchHelper = A.Fake<IMatchHelper>();
            A.CallTo(() => matchHelperFactory.GetMatchHelper(winner, loser)).Returns(matchHelper);
            return matchHelper;
        }

        private void SetPlayerWinner(IBattleshipsPlayer player, IMatchHelper matchHelper)
        {
            A.CallTo(() => matchHelper.GetWinner()).Returns(player);
            A.CallTo(() => playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(player)).Returns(winnerPlayerRecord);
        }

        private void SetPlayerLoser(IBattleshipsPlayer player, IMatchHelper matchHelper)
        {
            A.CallTo(() => matchHelper.GetLoser()).Returns(player);
            A.CallTo(() => playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(player)).Returns(loserPlayerRecord);
        }

        [Test]
        public void Runs_find_winner_correct_number_of_times()
        {
            // Given
            var matchHelper = SetUpMatchHelper(playerOne, playerTwo);
            SetPlayerWinnerCounter(50, matchHelper);
            SetPlayerLoserCounter(19, matchHelper);
            A.CallTo(() => matchHelper.PlayerOneCounter).Returns(1);

            // When
            runner.GetMatchResult(playerOne, playerTwo, 69);

            // Then
            A.CallTo(() => matchHelper.IncrementWinnerCounter(A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(69));
        }

        [Test]
        public void Draw_plays_one_more_round()
        {
            // Given
            var matchHelper = SetUpMatchHelper(playerOne, playerTwo);
            SetPlayerWinnerCounter(50, matchHelper);
            SetPlayerLoserCounter(19, matchHelper);

            // When
            runner.GetMatchResult(playerOne, playerTwo, 69);

            // Then
            A.CallTo(() => matchHelper.IncrementWinnerCounter(A<IBattleshipsPlayer>._)).MustHaveHappened(Repeated.Exactly.Times(70));
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<int[]> Games()
        {
            yield return new[] { 1, 2 };
            yield return new[] { 2, 1 };
        }

        private void PlayerIsValid(IBattleshipsPlayer player)
        {
            SetPlayerValid(player, true);
        }

        private void PlayerIsInvalid(IBattleshipsPlayer player)
        {
            SetPlayerValid(player, false);
        }

        private void SetPlayerValid(IBattleshipsPlayer player, bool isValid)
        {
            var shipPlacements = A.Fake<IShipsPlacement>();
            A.CallTo(() => shipsPlacementFactory.GetShipsPlacement(player)).Returns(shipPlacements);
            A.CallTo(() => shipPlacements.IsValid()).Returns(isValid);
        }

        private void ShipsAllHit(IBattleshipsPlayer player)
        {
            SetPlayerShipsAllHit(player, true);
        }

        private void ShipsNotAllHit(IBattleshipsPlayer player)
        {
            SetPlayerShipsAllHit(player, false);
        }

        private void SetPlayerShipsAllHit(IBattleshipsPlayer player, bool isAllHit)
        {
            var shipPlacements = shipsPlacementFactory.GetShipsPlacement(player);
            A.CallTo(() => shipPlacements.AllHit()).Returns(isAllHit);
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

        private IBattleshipsPlayer FindWinner()
        {
            return runner.FindWinner(playerOne, playerTwo);
        }

        private Constraint IsPlayer(int number)
        {
            return Is.EqualTo(Player(number));
        }
    }
}