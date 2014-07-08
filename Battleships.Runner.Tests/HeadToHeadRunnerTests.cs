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
        private IShipPositionValidator shipPositionValidator;
        private IMoveChecker player1MoveChecker;
        private IMoveChecker player2MoveChecker;
        private IMoveCheckerFactory moveCheckerFactory;
        private ICellsHitByPlayerChecker cellsHitByPlayerChecker;

        [SetUp]
        public void SetUp()
        {
            shipPositionValidator = A.Fake<IShipPositionValidator>();
            player1 = A.Fake<IBattleshipsPlayer>();
            player2 = A.Fake<IBattleshipsPlayer>();
            player1MoveChecker = A.Fake<IMoveChecker>();
            player2MoveChecker = A.Fake<IMoveChecker>();
            moveCheckerFactory = A.Fake<IMoveCheckerFactory>();
            runner = new HeadToHeadRunner(shipPositionValidator, moveCheckerFactory);
            cellsHitByPlayerChecker = A.Fake<CellsHitByPlayerChecker>();
        }

        [TestCaseSource("Games")]
        public void Player_loses_if_ship_positions_invalid(int expectedWinner, int expectedLoser)
        {
            // Given
            var losingShipPositions = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => Player(expectedLoser).GetShipPositions()).Returns(losingShipPositions);
            A.CallTo(() => shipPositionValidator.IsValid(losingShipPositions)).Returns(false);

            var winningShipPositions = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => Player(expectedWinner).GetShipPositions()).Returns(winningShipPositions);
            A.CallTo(() => shipPositionValidator.IsValid(winningShipPositions)).Returns(true);

            // When
            var winner = RunGame();

            // Then
            Assert.That(winner, IsPlayer(expectedWinner));
        }

        [TestCaseSource("Games")]
        public void Player_loses_if_it_throws_exception(int expectedWinner, int expectedLoser)
        {
            // Given
            A.CallTo(() => Player(expectedLoser).GetShipPositions()).Throws(() => new Exception());

            var winningShipPositions = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => Player(expectedWinner).GetShipPositions()).Returns(winningShipPositions);
            A.CallTo(() => shipPositionValidator.IsValid(winningShipPositions)).Returns(true);

            // When
            var winner = RunGame();

            // Then
            Assert.That(winner, IsPlayer(expectedWinner));
        }

        [Test]
        public void Player_one_wins()
        {
            // Given
            var player1ShipPositions = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => player1.GetShipPositions()).Returns(player1ShipPositions);
            var player2ShipPositions = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => player2.GetShipPositions()).Returns(player2ShipPositions);
            A.CallTo(() => shipPositionValidator.IsValid(player1ShipPositions)).Returns(true);
            A.CallTo(() => shipPositionValidator.IsValid(player2ShipPositions)).Returns(true);

            A.CallTo(() => moveCheckerFactory.GetMoveChecker(player2ShipPositions)).
              Returns(player1MoveChecker);

            A.CallTo(() => player1MoveChecker.AllHit()).Returns(true);

            // When
            var winner = RunGame();

            // Then
            winner.Should().Be(player1);
        }

        [Test]
        public void Player_two_wins()
        {
            // Given
            var player1ShipPositions = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => player1.GetShipPositions()).Returns(player1ShipPositions);
            var player2ShipPositions = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => player2.GetShipPositions()).Returns(player2ShipPositions);
            A.CallTo(() => shipPositionValidator.IsValid(player1ShipPositions)).Returns(true);
            A.CallTo(() => shipPositionValidator.IsValid(player2ShipPositions)).Returns(true);

            A.CallTo(() => moveCheckerFactory.GetMoveChecker(player2ShipPositions)).
              Returns(player1MoveChecker);
            A.CallTo(() => moveCheckerFactory.GetMoveChecker(player1ShipPositions)).
              Returns(player2MoveChecker);

            A.CallTo(() => player1MoveChecker.AllHit()).Returns(false);
            A.CallTo(() => player2MoveChecker.AllHit()).Returns(true);

            // When
            var winner = RunGame();

            // Then
            winner.Should().Be(player2);
        }

        [Test]
        public void Player_loses_on_timeout() {}

        [Test]
        // Tested in MoveCheckerTests
        public void Shot_result_is_reported_correctly_to_player() {}

        [Test]
        // Tested in MoveCheckerTests
        public void Opponent_shot_is_reported_correctly_to_player() {}

        private static IEnumerable<int[]> Games()
        {
            yield return new[] { 1, 2 };
            yield return new[] { 2, 1 };
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