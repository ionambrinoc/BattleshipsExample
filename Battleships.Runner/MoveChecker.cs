namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;

    internal class MoveChecker
    {
        private readonly IEnumerable<IShipPosition> playerOShipPositions;
        private readonly IGridSquare target;

        public MoveChecker(IEnumerable<IShipPosition> playerOShipPositions, IGridSquare target)
        {
            this.playerOShipPositions = playerOShipPositions;
            this.target = target;
        }

        public bool CheckResultOfMove()
        {
            foreach (var shipPosition in playerOShipPositions)
            {
                if (IsTargetInShip(shipPosition))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsTargetInShip(IShipPosition shipPosition)
        {
            if (IsShipHorizontal(shipPosition))
            {
                if (target.Row == shipPosition.EndingSquare.Row)
                {
                    return RangeChecker(target.Column, shipPosition.EndingSquare.Column, shipPosition.StartingSquare.Column);
                }
            }
            else
            {
                if (target.Column == shipPosition.EndingSquare.Column)
                {
                    return RangeChecker(target.Row, shipPosition.EndingSquare.Row, shipPosition.StartingSquare.Row);
                }
            }
        }

        private bool RangeChecker(int target, int shipEnd, int shipStart)
        {
            return (target <= shipEnd && target >= shipStart) || (target >= shipEnd && target <= shipStart);
        }

        private bool IsShipHorizontal(IShipPosition shipPosition)
        {
            return shipPosition.StartingSquare.Row == shipPosition.EndingSquare.Row;
        }
    }
}