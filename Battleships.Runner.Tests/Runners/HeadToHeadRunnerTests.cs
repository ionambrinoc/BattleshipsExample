namespace Battleships.Runner.Tests.Runners
{
    using Battleships.Player;
    using Battleships.Runner.Factories;
    using Battleships.Runner.Models;
    using Battleships.Runner.Runners;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
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
            shipsPlacementFactory = A.Fake<IShipsPlacementFactory>();
            playerOne = GetNewValidPlayer();
            playerTwo = GetNewValidPlayer();
            runner = new HeadToHeadRunner(shipsPlacementFactory);
        }

        [TestCaseSource("Games")]
        public void Player_loses_if_ship_positions_invalid(int expectedWinner, int expectedLoser)
        {
            // Given
            PlayerIsInvalid(Player(expectedLoser));

            // When
            var result = GetResult();

            // Then
            result.Winner.Should().Be(Player(expectedWinner));
            result.ResultType.Should().Be(ResultType.ShipPositionsInvalid);
        }

        [Test]
        public void Player_two_wins_if_both_invalid()
        {
            // Given
            PlayerIsInvalid(playerOne);
            PlayerIsInvalid(playerTwo);

            // When
            var result = GetResult();

            // Then
            result.Winner.Should().Be(playerTwo);
            result.ResultType.Should().Be(ResultType.ShipPositionsInvalid);
        }

        [TestCaseSource("Games")]
        public void Player_with_all_ships_hit_loses_with_default_result_type(int expectedWinner, int expectedLoser)
        {
            // Given
            ShipsNotAllHit(Player(expectedWinner));
            ShipsAllHit(Player(expectedLoser));

            // When
            var result = GetResult();

            // Then
            result.Winner.Should().Be(Player(expectedWinner));
            result.ResultType.Should().Be(ResultType.Default);
        }

        [TestCaseSource("Games")]
        public void Player_loses_on_timeout_with_timeout_result_type(int expectedWinner, int expectedLoser)
        {
            // Given
            A.CallTo(() => Player(expectedLoser).HasTimedOut()).Returns(true);

            // When
            var result = GetResult();

            // Then
            result.Winner.Should().Be(Player(expectedWinner));
            result.ResultType.Should().Be(ResultType.Timeout);
        }

        [TestCaseSource("Games")]
        public void Player_who_throws_an_exception_loses(int expectedWinner, int expectedLoser)
        {
            // Given
            A.CallTo(() => Player(expectedLoser).SelectTarget()).Throws(new BotException("message", new Exception(), Player(expectedLoser)));

            // When
            var result = GetResult();

            // Then
            result.Winner.Should().Be(Player(expectedWinner));
            result.ResultType.Should().Be(ResultType.OpponentThrewException);
        }

        [Test]
        public void Two_consecutive_games_do_not_interfere()
        {
            // Given
            ShipsNotAllHit(playerOne);
            ShipsAllHit(playerTwo);
            var firstWinner = GetResult().Winner;

            ShipsAllHit(playerOne);
            ShipsNotAllHit(playerTwo);

            // When
            var secondWinner = GetResult().Winner;

            // Then
            firstWinner.Should().Be(playerOne);
            secondWinner.Should().Be(playerTwo);
        }

        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<int[]> Games()
        {
            yield return new[] { 1, 2 };
            yield return new[] { 2, 1 };
        }

        private IBattleshipsPlayer GetNewValidPlayer()
        {
            var player = A.Fake<IBattleshipsPlayer>();
            SetPlayerValidity(player, true);
            return player;
        }

        private void PlayerIsInvalid(IBattleshipsPlayer player)
        {
            SetPlayerValidity(player, false);
        }

        private void SetPlayerValidity(IBattleshipsPlayer player, bool isValid)
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
            return number == 1 ? playerOne : playerTwo;
        }

        private GameResult GetResult()
        {
            return runner.FindWinner(playerOne, playerTwo);
        }
    }
}
