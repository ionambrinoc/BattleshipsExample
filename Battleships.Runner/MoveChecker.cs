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
                    if (target.Column <= shipPosition.EndingSquare.Column && target.Column >= shipPosition.StartingSquare.Column)
                    {
                        return true;
                    }
                    if (target.Column >= shipPosition.EndingSquare.Column && target.Column <= shipPosition.StartingSquare.Column)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (target.Column == shipPosition.EndingSquare.Column)
                {
                    if (target.Row <= shipPosition.EndingSquare.Row && target.Row >= shipPosition.StartingSquare.Row)
                    {
                        return true;
                    }
                    if (target.Row >= shipPosition.EndingSquare.Row && target.Row <= shipPosition.StartingSquare.Row)
                    {
                        return true;
                    }
                }
            }
        }

        private bool IsShipHorizontal(IShipPosition shipPosition)
        {
            return shipPosition.StartingSquare.Row == shipPosition.EndingSquare.Row;
        }
    }
}