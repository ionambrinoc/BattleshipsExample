namespace Battleships.Runner
{
    public interface IMoveCheckerFactory
    {
        IMoveChecker GetMoveChecker(IShipsPlacement shipsPlacement);
    }

    public class MoveCheckerFactory
    {
        public IMoveChecker GetMoveChecker(IShipsPlacement shipsPlacement)
        {
            return new MoveChecker(shipsPlacement, new CellsHitByPlayerChecker());
        }
    }
}