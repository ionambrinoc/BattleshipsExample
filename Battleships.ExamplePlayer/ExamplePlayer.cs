namespace Battleships.ExamplePlayer
{
    using Battleships.Player.Interface;
    using System;
    using System.Collections.Generic;

    public class ExamplePlayer : IBattleshipsBot
    {
        internal IGridSquare LastTarget;
        private readonly HashSet<IGridSquare> shipsHit = new HashSet<IGridSquare>();
        private readonly HashSet<IGridSquare> myShips = new HashSet<IGridSquare>();

        public enum Direction
        {
            Horizontal,
            Vertical
        };

        public enum Ship
        {
            Carrier,
            Battleship,
            Submarine,
            Cruiser,
            Destroyer
        };

        public string Name
        {
            get { return "Ion's Awesome Player"; }
        }

        public int CountGridSquares()
        {
            return myShips.Count;
        }

        public HashSet<IGridSquare> GetGrid()
        {
            return myShips;
        }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            var shipPositions = new List<IShipPosition>();
            var canNotPlaceShip = true;
            var deniedSquares = new HashSet<IGridSquare>();
            var randomizer = new Random();

            foreach (Ship ship in Enum.GetValues(typeof(Ship)))
            {
                var startCol = 1;
                var startRow = 'A';
                var direction = randomizer.Next(0, 2) == 0 ? Direction.Horizontal : Direction.Vertical;
                var shipLength = GetShipLength(ship);
                deniedSquares.UnionWith(new HashSet<IGridSquare>(myShips));
                var shipSquaresCandidate = new HashSet<IGridSquare>();

                while (canNotPlaceShip)
                {
                    startCol = direction == Direction.Horizontal ? randomizer.Next(1, 10 - shipLength) : randomizer.Next(1, 10);
                    startRow = (char)(direction == Direction.Horizontal ? randomizer.Next('A', 'J') : randomizer.Next('A', 'J' - shipLength));

                    direction = randomizer.Next(0, 2) == 0 ? Direction.Horizontal : Direction.Vertical;

                    shipSquaresCandidate = GetShipSquares(startRow, startCol, direction, shipLength);

                    var intersection = new HashSet<IGridSquare>(shipSquaresCandidate);
                    intersection.IntersectWith(deniedSquares);

                    canNotPlaceShip = intersection.Count > 0;
                }
                shipPositions.Add(PlaceShip(startRow, startCol, direction, ship));
                deniedSquares.UnionWith(AddDeniedSquares(startRow, startCol, direction, ship));
                canNotPlaceShip = true;
            }
            return shipPositions;
        }

        public HashSet<IGridSquare> AddDeniedSquares(char startRow, int startCol, Direction direction, Ship ship)
        {
            var deniedSquares = new HashSet<IGridSquare>();
            var shipLength = GetShipLength(ship);

            if (direction == Direction.Vertical)
            {
                for (var r = (char)Math.Max(startRow - 1, 'A'); r <= (char)Math.Min(startRow + shipLength + 1, 'J'); r++)
                {
                    deniedSquares.Add(new GridSquare(r, Math.Max(startCol - 1, 0)));
                    deniedSquares.Add(new GridSquare(r, startCol));
                    deniedSquares.Add(new GridSquare(r, Math.Min(startCol + 1, 10)));
                }
            }
            else
            {
                for (var c = Math.Max(startCol - 1, 0); c <= Math.Min(startCol + shipLength + 1, 10); c++)
                {
                    deniedSquares.Add(new GridSquare((char)Math.Max(startRow - 1, 'A'), c));
                    deniedSquares.Add(new GridSquare(startRow, c));
                    deniedSquares.Add(new GridSquare((char)Math.Min(startRow + 1, 'J'), c));
                }
            }
            return deniedSquares;
        }

        public IGridSquare SelectTarget()
        {
            var nextTarget = GetNextTarget();
            LastTarget = nextTarget;
            return nextTarget;
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            if (wasHit)
            {
                shipsHit.Add(square);
            }
        }

        public void HandleOpponentsShot(IGridSquare square) {}

        private static ShipPosition GetShipPosition(char startRow, int startColumn, char endRow, int endColumn)
        {
            return new ShipPosition(new GridSquare(startRow, startColumn), new GridSquare(endRow, endColumn));
        }

        private static int GetShipLength(Ship ship)
        {
            var shipLength = 0;
            switch (ship)
            {
                case Ship.Carrier:
                    shipLength = 4;
                    break;
                case Ship.Battleship:
                    shipLength = 3;
                    break;
                case Ship.Cruiser:
                    shipLength = 2;
                    break;
                case Ship.Submarine:
                    shipLength = 2;
                    break;
                case Ship.Destroyer:
                    shipLength = 1;
                    break;
            }
            return shipLength;
        }

        private ShipPosition PlaceShip(char row, int col, Direction direction, Ship ship)
        {
            var shipLength = GetShipLength(ship);

            myShips.UnionWith(GetShipSquares(row, col, direction, shipLength));
            if (direction == Direction.Vertical)
            {
                return GetShipPosition(row, col, (char)(row + shipLength), col);
            }
            return GetShipPosition(row, col, row, col + shipLength);
        }

        private HashSet<IGridSquare> GetShipSquares(char row, int col, Direction direction, int shipLength)
        {
            var shipSquares = new HashSet<IGridSquare>();

            if (direction == Direction.Vertical)
            {
                for (var r = row; r <= (char)(row + shipLength); r++)
                {
                    shipSquares.Add(new GridSquare(r, col));
                }
                return shipSquares;
            }
            for (var c = col; c <= col + shipLength; c++)
            {
                shipSquares.Add(new GridSquare(row, c));
            }
            return shipSquares;
        }

        private IGridSquare GetNextTarget()
        {
            if (LastTarget == null)
            {
                return new GridSquare('A', 1);
            }

            var row = LastTarget.Row;
            var col = LastTarget.Column + 1;
            if (LastTarget.Column != 10)
            {
                return new GridSquare(row, col);
            }

            row = (char)(row + 1);
            if (row > 'J')
            {
                row = 'A';
            }
            col = 1;
            return new GridSquare(row, col);
        }
    }
}