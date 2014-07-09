namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IShipsPlacementFactory
    {
        IShipsPlacement GetShipPlacement(IBattleshipsPlayer player);
    }

    public class ShipsPlacementFactory
    {
        public IShipsPlacement GetShipPlacement(IBattleshipsPlayer player)
        {
            return new ShipsPlacement(player);
        }
    }
}