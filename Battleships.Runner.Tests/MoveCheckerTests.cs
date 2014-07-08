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
        private MoveChecker moveChecker;

        [SetUp]
        public void SetUp()
        {
            cellsHitByPlayer = A.Fake<ICellsHitByPlayerChecker>();
            moveChecker = new MoveChecker(new List<IShipPosition>
                                          {
                                              new ShipPosition(new GridSquare('A', 1), new GridSquare('B', 1)),
                                              new ShipPosition(new GridSquare('A', 3), new GridSquare('C', 3)),
                                              new ShipPosition(new GridSquare('E', 1), new GridSquare('E', 3)),
                                              new ShipPosition(new GridSquare('G', 1), new GridSquare('G', 5))
                                          }, cellsHitByPlayer);
        }

        [Test]
        public void Hit_to_horizontal_ship_returns_true()
        {
            //Given

            //When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('E', 1));

            //Then
            Assert.That(isHit);
        }

        [Test]
        public void Hit_to_vertical_ship_returns_true()
        {
            //Given

            //When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('A', 1));

            //Then
            Assert.That(isHit);
        }

        [Test]
        public void Miss_returns_false()
        {
            //Given

            //When
            var isHit = moveChecker.CheckResultOfMove(new GridSquare('C', 1));

            //Then
            Assert.That(!isHit);
        }
    }
}