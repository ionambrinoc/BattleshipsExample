namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IMoveChecker
    {
        bool CheckResultOfMove(IGridSquare target);
        void AddToCellsHit(IGridSquare target);
        bool AllHit();
    }

    public class MoveChecker : IMoveChecker
    {
        private readonly IShipsPlacement opposingPlayerShipsPlacement;
        private readonly ICellsHitByPlayerChecker cellsOfShipHitChecker;

        public MoveChecker(IShipsPlacement opposingPlayerShipsPlacement, ICellsHitByPlayerChecker cellsOfShipHitChecker)
        {
            this.opposingPlayerShipsPlacement = opposingPlayerShipsPlacement;
            this.cellsOfShipHitChecker = cellsOfShipHitChecker;
        }

        public bool CheckResultOfMove(IGridSquare target)
        {
            foreach (var shipPosition in opposingPlayerShipsPlacement.GetShipPositions())
            {
                if (IsTargetInShip(shipPosition, target))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddToCellsHit(IGridSquare target)
        {
            cellsOfShipHitChecker.AddCell(target);
        }

        public bool AllHit()
        {
            return cellsOfShipHitChecker.AllHit();
        }

        private bool IsTargetInShip(IShipPosition shipPosition, IGridSquare target)
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

        private bool IsInRange(int target, int shipEnd, int shipStart)
        {
            return (target <= shipEnd && target >= shipStart) || (target >= shipEnd && target <= shipStart);
        }

        private bool IsShipHorizontal(IShipPosition shipPosition)
        {
            return shipPosition.StartingSquare.Row == shipPosition.EndingSquare.Row;
        }
    }
}