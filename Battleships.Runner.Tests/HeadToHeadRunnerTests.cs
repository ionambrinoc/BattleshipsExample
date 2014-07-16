namespace Battleships.Runner.Tests
{
    using Battleships.Player;
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

        [SetUp]
        public void SetUp()
        {
            playerOne = A.Fake<IBattleshipsPlayer>();
            playerTwo = A.Fake<IBattleshipsPlayer>();
            shipsPlacementFactory = A.Fake<IShipsPlacementFactory>();
            runner = new HeadToHeadRunner(shipsPlacementFactory);
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
        public void Player_loses_on_timeout_and_winType_is_timeout()
        {
            // Given
            // This will cause one of the players to timeout, since neither can win with this strategy, and invalid moves are not possible
            A.CallTo(() => player1.SelectTarget()).Returns(new GridSquare('A', 1));
            A.CallTo(() => player2.SelectTarget()).Returns(new GridSquare('A', 1));
            PlayerIsValid(player1);
            PlayerIsValid(player2);

            //When
            var result = runner.FindWinner(player1, player2).WinType;

            //Then
            Assert.AreEqual(result, WinTypes.Timeout);
        }

        [Test]
        public void Player_loses_on_invalid_move_and_winType_is_invalid()
        {
            //Given
            PlayerIsInvalid(player1);

            //When
            var result = runner.FindWinner(player1, player2).WinType;

            //Then
            Assert.AreEqual(result, WinTypes.Invalid);
        }

        [Test]
        public void A_normal_game_returns_winType_as_default()
        {
            //Given
            PlayerIsValid(player1);
            PlayerIsValid(player2);
            ShipsAllHit(player1);

            //when
            var result = runner.FindWinner(player1, player2).WinType;

            //Then
            Assert.AreEqual(result, WinTypes.Default);
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
            return runner.FindWinner(player1, player2).Winner;
        }

        private Constraint IsPlayer(int number)
        {
            return Is.EqualTo(Player(number));
        }
    }
}