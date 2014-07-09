namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class MoveCheckerTests
    {
        private ICellsHitByPlayerChecker cellsHitByPlayer;
        private IShipsPlacement shipsPlacement;
        private MoveChecker moveChecker;

        [SetUp]
        public void SetUp()
        {
            cellsHitByPlayer = A.Fake<ICellsHitByPlayerChecker>();
            shipsPlacement = A.Fake<IShipsPlacement>();
            moveChecker = new MoveChecker(shipsPlacement, cellsHitByPlayer);
        }

        [Test]
        public void Shot_on_horizontal_ship_is_hit()
        {
            // Given
            FakeGetShipPositions(new GridSquare('E', 1), new GridSquare('E', 3));

            // When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('E', 1));

            // Then
            Assert.That(isHit, "Ship should have been hit but was not");
        }

        [Test]
        public void Shot_on_vertical_ship_is_hit()
        {
            // Given
            FakeGetShipPositions(new GridSquare('A', 1), new GridSquare('B', 1));

            // When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('B', 1));

            // Then
            Assert.That(isHit, "Ship should have been hit but was not");
        }

        [Test]
        public void Miss_returns_false()
        {
            // Given
            FakeGetShipPositions(new GridSquare('G', 1), new GridSquare('G', 5));

            // When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('C', 1));

            // Then
            Assert.That(isHit, Is.EqualTo(false), "Ship should have been missed but was hit");
        }

        [Test]
        public void Shot_off_the_board_returns_false()
        {
            // Given
            FakeGetShipPositions(new GridSquare('G', 1), new GridSquare('G', 5));

            // When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('A', 100));

            // Then
            Assert.That(isHit, Is.EqualTo(false), "Shot off the board should return false");
        }

        [Test]
        public void Shot_in_the_middle_of_ship_returns_true()
        {
            // Given
            FakeGetShipPositions(new GridSquare('C', 1), new GridSquare('C', 5));

            // When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('C', 3));

            // Then
            Assert.That(isHit, "Ship should have been hit but was not");
        }

        private void FakeGetShipPositions(IGridSquare startSquare, IGridSquare endSquare)
        {
            A.CallTo(() => shipsPlacement.GetShipPositions()).Returns(new List<IShipPosition>
                                                                      {
                                                                          new ShipPosition(startSquare, endSquare)
                                                                      });
        }
    }
}