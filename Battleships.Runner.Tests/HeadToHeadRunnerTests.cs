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
        public void Player_loses_on_timeout() {}

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