namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;

    public interface IMoveCheckerFactory
    {
        IMoveChecker GetMoveChecker(IEnumerable<IShipPosition> shipPositions);
    }

    public class MoveCheckerFactory
    {
        public IMoveChecker GetMoveChecker(IEnumerable<IShipPosition> shipPositions)
        {
            return new MoveChecker(shipPositions, new CellsHitByPlayerChecker());
        }
    }
}