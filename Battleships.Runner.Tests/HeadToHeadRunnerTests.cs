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
        private IBattleshipsPlayer player1;
        private IBattleshipsPlayer player2;
        private IMoveChecker player1MoveChecker;
        private IMoveChecker player2MoveChecker;
        private IMoveCheckerFactory moveCheckerFactory;
        private IShipsPlacementFactory shipsPlacementFactory;

        [SetUp]
        public void SetUp()
        {
            player1 = A.Fake<IBattleshipsPlayer>();
            player2 = A.Fake<IBattleshipsPlayer>();
            player1MoveChecker = A.Fake<IMoveChecker>();
            player2MoveChecker = A.Fake<IMoveChecker>();

            moveCheckerFactory = A.Fake<IMoveCheckerFactory>();
            shipsPlacementFactory = A.Fake<IShipsPlacementFactory>();
            runner = new HeadToHeadRunner(shipsPlacementFactory, moveCheckerFactory);
        }

        [TestCaseSource("Games")]
        public void Player_loses_if_ship_positions_invalid_or_player_throws_exception(int expectedWinner, int expectedLoser)
        {
            // Given
            InitialiseShips(Player(expectedLoser), false, false);
            InitialiseShips(Player(expectedWinner), true, false);

            // When
            var winner = RunGame();

            // Then
            Assert.That(winner, IsPlayer(expectedWinner));
        }

        [TestCaseSource("Games")]
        public void Player_two_wins_if_both_invalid(int aPlayer, int anotherPlayer)
        {
            // Given
            InitialiseShips(Player(anotherPlayer), false, false);
            InitialiseShips(Player(aPlayer), false, false);

            // When
            var winner = RunGame();

            // Then
            winner.Should().Be(player2);
        }

        [TestCaseSource("Games")]
        public void Correct_player_wins(int expectedWinner, int expectedLoser)
        {
            // Given
            InitialiseShips(Player(expectedWinner), true, false);
            InitialiseShips(Player(expectedLoser), true, true);

            // When
            var winner = RunGame();

            // Then
            winner.Should().Be(Player(expectedWinner));
        }

        [Test]
        public void Player_loses_on_timeout() {}

        private static IEnumerable<int[]> Games()
        {
            yield return new[] { 1, 2 };
            yield return new[] { 2, 1 };
        }

        private void InitialiseShips(IBattleshipsPlayer player, bool valid, bool allPlayersShipsHit)
        {
            var shipPlacements = A.Fake<IShipsPlacement>();
            A.CallTo(() => shipsPlacementFactory.GetShipPlacement(player)).Returns(shipPlacements);
            A.CallTo(() => shipPlacements.IsValid()).Returns(valid);

            var moveChecker = player.Equals(Player(1)) ? player2MoveChecker : player1MoveChecker;
            A.CallTo(() => moveCheckerFactory.GetMoveChecker(shipPlacements)).Returns(moveChecker);
            A.CallTo(() => moveChecker.AllHit()).Returns(allPlayersShipsHit);
        }

        private IBattleshipsPlayer Player(int number)
        {
            switch (number)
            {
                case 1:
                    return player1;
                case 2:
                    return player2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IBattleshipsPlayer RunGame()
        {
            return runner.RunGame(player1, player2);
        }

        private Constraint IsPlayer(int number)
        {
            return Is.EqualTo(Player(number));
        }
    }
}