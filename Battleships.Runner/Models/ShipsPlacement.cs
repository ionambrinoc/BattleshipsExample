namespace Battleships.Runner.Models
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
        private readonly List<IShip> positions;
        private readonly HashSet<IGridSquare> cellsOfShipsHit = new HashSet<IGridSquare>();

        public ShipsPlacement(IBattleshipsPlayer player, IShipFactory shipFactory)
        {
            try
            {
                var shipPositions = player.GetShipPositions();
                positions = shipFactory.GetShips(shipPositions);
            }
            catch (Exception)
            {
                positions = null;
            }
        }

        public bool IsValid()
        {
            var shipsAvailableOfSize = new Dictionary<int, int> { { 2, 1 }, { 3, 2 }, { 4, 1 }, { 5, 1 } };
            var coordinatesAdjacentToShip = new bool[12, 12];

            if (positions == null)
            {
                return false;
            }

            foreach (var shipPosition in positions)
            {
                if (shipPosition.IsValid && shipsAvailableOfSize[shipPosition.ShipLength] != 0
                    && IsPositionValid(shipPosition, coordinatesAdjacentToShip))
                {
                    shipsAvailableOfSize[shipPosition.ShipLength] -= 1;
                    OccupyGridSquares(shipPosition, coordinatesAdjacentToShip);
                }
                else
                {
                    return false;
                }
            }
            return shipsAvailableOfSize.All(ship => ship.Value == 0);
        }

        public bool IsHit(IGridSquare target)
        {
            if (positions.Any(ship => ship.IsTargetInShip(target)))
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

        private void OccupyGridSquares(IShip shipPosition, bool[,] coordinatesAdjacentToShip)
        {
            for (var i = shipPosition.StartingSquare.Column - 1; i <= shipPosition.EndingSquare.Column + 1; i++)
            {
                for (var j = GetIndexFromChar(shipPosition.StartingSquare.Row) - 1; j <= GetIndexFromChar(shipPosition.EndingSquare.Row) + 1; j++)
                {
                    coordinatesAdjacentToShip[i, j] = true;
                }
            }
        }

        private bool IsPositionValid(IShip shipPosition, bool[,] coordinatesAdjacentToShip)
        {
            for (var i = shipPosition.StartingSquare.Column; i <= shipPosition.EndingSquare.Column; i++)
            {
                for (var j = GetIndexFromChar(shipPosition.StartingSquare.Row); j <= GetIndexFromChar(shipPosition.EndingSquare.Row); j++)
                {
                    if (coordinatesAdjacentToShip[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int GetIndexFromChar(char row)
        {
            return row - 'A' + 1;
        }
    }
}