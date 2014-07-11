namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IShipsPlacementFactory
    {
        IShipsPlacement GetShipsPlacement(IBattleshipsPlayer player);
    }

    public class ShipsPlacementFactory : IShipsPlacementFactory
    {
        public IShipsPlacement GetShipsPlacement(IBattleshipsPlayer player)
        {
            return new ShipsPlacement(player);
        }
    }
}
