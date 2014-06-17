namespace Battleships.ExamplePlayer
{
    using Battleships.Player;
    using System;
    using System.Collections.Generic;

    public class ExamplePlayer : IBattleshipsPlayer
    {
        public string Name
        {
            get { return "Example Player"; }
        }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            return new List<IShipPosition>
                   {
                       GetShipPosition('A', 1, 'A', 5),
                       GetShipPosition('C', 1, 'C', 4),
                       GetShipPosition('E', 1, 'E', 3),
                       GetShipPosition('G', 1, 'G', 3),
                       GetShipPosition('I', 1, 'I', 2)
                   };
        }

        public IGridSquare SelectTarget()
        {
            const string rows = "ABCDEFGHIJ";
            var random = new Random();
            var row = rows[random.Next(1, 11)];
            var column = random.Next(1, 11);
            return new GridSquare(row, column);
        }

        public void HandleShotResult(IGridSquare square, bool wasHit) {}

        public void HandleOpponentsShot(IGridSquare square) {}

        private static ShipPosition GetShipPosition(char startRow, int startColumn, char endRow, int endColumn)
        {
            return new ShipPosition(new GridSquare(startRow, startColumn), new GridSquare(endRow, endColumn));
        }
    }
}