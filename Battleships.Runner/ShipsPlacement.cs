namespace Battleships.Runner
{
    using Battleships.Player;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IShipsPlacement
    {
        bool IsValid();
        bool IsHit(IGridSquare target);
        bool AllHit();
    }

    public class ShipsPlacement : IShipsPlacement
    {
        private const int TotalNumberOfShipCells = 17;
        private readonly IEnumerable<IShipPosition> positions;
        private readonly HashSet<IGridSquare> cellsOfShipsHit;

        public ShipsPlacement(IBattleshipsPlayer player)
        {
            try
            {
                positions = player.GetShipPositions();
            }
            catch (Exception)
            {
                positions = null;
            }

            cellsOfShipsHit = new HashSet<IGridSquare>();
        }

        public bool IsValid()
        {
            var shipsAvailableOfSize = new Dictionary<int, int> { { 2, 1 }, { 3, 2 }, { 4, 1 }, { 5, 1 } };
            var gridSquaresOccupied = new bool[10, 10];

            try
            {
                foreach (var shipPosition in positions)
                {
                    var shipLength = GetShipLength(shipPosition);
                    if (shipsAvailableOfSize.ContainsKey(shipLength) && shipsAvailableOfSize[shipLength] != 0
                        && IsNotAdjacentToPreviousShips(shipPosition, gridSquaresOccupied))
                    {
                        shipsAvailableOfSize[shipLength] -= 1;
                        OccupyGridSquares(shipPosition, gridSquaresOccupied);
                    }
                    else
                    {
                        return false;
                    }
                }

                return shipsAvailableOfSize.Aggregate(true, (current, shipAvailable) => current && shipAvailable.Value == 0);
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public bool IsHit(IGridSquare target)
        {
            if (positions.Any(ship => IsTargetInShip(ship, target)))
            {
                cellsOfShipsHit.Add(target);
                return true;
            }
            return false;
        }

        public bool AllHit()
        {
            return cellsOfShipsHit.Count == TotalNumberOfShipCells;
        }

        private static bool IsTargetInShip(IShipPosition shipPosition, IGridSquare target)
        {
            if (IsShipHorizontal(shipPosition))
            {
                if (target.Row == shipPosition.EndingSquare.Row)
                {
                    return IsInRange(target.Column, shipPosition.EndingSquare.Column, shipPosition.StartingSquare.Column);
                }
            }
            else
            {
                if (target.Column == shipPosition.EndingSquare.Column)
                {
                    return IsInRange(target.Row, shipPosition.EndingSquare.Row, shipPosition.StartingSquare.Row);
                }
            }
            return false;
        }

        private static bool IsInRange(int target, int shipEnd, int shipStart)
        {
            return (target <= shipEnd && target >= shipStart) || (target >= shipEnd && target <= shipStart);
        }

        private static bool IsShipHorizontal(IShipPosition shipPosition)
        {
            return shipPosition.StartingSquare.Row == shipPosition.EndingSquare.Row;
        }

        private static bool IsShipVertical(IShipPosition shipPosition)
        {
            return shipPosition.StartingSquare.Column == shipPosition.EndingSquare.Column;
        }

        private void OccupyGridSquares(IShipPosition shipPosition, bool[,] gridSquaresOccupied)
        {
            var orientedShipPosition = GridSquaresInCorrectOrder(shipPosition.StartingSquare, shipPosition.EndingSquare);

            for (var i = orientedShipPosition.StartingSquare.Column - 2; i <= orientedShipPosition.EndingSquare.Column; i++)
            {
                for (var j = (orientedShipPosition.StartingSquare.Row - 'A') - 1; j <= (orientedShipPosition.EndingSquare.Row - 'A') + 1; j++)
                {
                    if (i >= 0 && j >= 0 && i < 10 && j < 10)
                    {
                        gridSquaresOccupied[i, j] = true;
                    }
                }
            }
        }

        private bool IsNotAdjacentToPreviousShips(IShipPosition shipPosition, bool[,] gridSquaresOccupied)
        {
            var adjacent = false;
            var orientedShipPosition = GridSquaresInCorrectOrder(shipPosition.StartingSquare, shipPosition.EndingSquare);

            for (var i = orientedShipPosition.StartingSquare.Column - 1; i <= orientedShipPosition.EndingSquare.Column - 1; i++)
            {
                for (var j = (orientedShipPosition.StartingSquare.Row - 'A'); j <= (orientedShipPosition.EndingSquare.Row - 'A'); j++)
                {
                    adjacent = gridSquaresOccupied[i, j] || adjacent;
                }
            }
            return !adjacent;
        }

        private IShipPosition GridSquaresInCorrectOrder(IGridSquare firstGridSquare, IGridSquare secondGridSquare)
        {
            if (firstGridSquare.Column <= secondGridSquare.Column && firstGridSquare.Row <= secondGridSquare.Row)
            {
                return new ShipPosition(firstGridSquare, secondGridSquare);
            }
            return new ShipPosition(secondGridSquare, firstGridSquare);
        }

        private int GetShipLength(IShipPosition shipPosition)
        {
            if (IsShipHorizontal(shipPosition))
            {
                return Math.Abs(shipPosition.StartingSquare.Column - shipPosition.EndingSquare.Column) + 1;
            }
            if (IsShipVertical(shipPosition))
            {
                return Math.Abs(shipPosition.StartingSquare.Row - shipPosition.EndingSquare.Row) + 1;
            }
            return 0;
        }
    }
}