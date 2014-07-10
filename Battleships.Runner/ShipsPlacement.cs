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
        public int NumberOfCellsHit;
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
            NumberOfCellsHit = 0;
        }

        public bool IsValid()
        {
            // TODO: Card 16 - Check players' ship positions are valid
            return positions != null;
        }

        public bool IsHit(IGridSquare target)
        {
            if (positions.Any(ship => IsTargetInShip(ship, target)))
            {
                cellsOfShipsHit.Add(target);
                NumberOfCellsHit = cellsOfShipsHit.Count;
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
    }
}