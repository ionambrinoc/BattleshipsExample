namespace Battleships.Runner.Factories
{
    using Battleships.Player;
    using Battleships.Runner.Models;

    public interface IShipsPlacementFactory
    {
        IShipsPlacement GetShipsPlacement(IBattleshipsPlayer player);
    }

    public class ShipsPlacementFactory : IShipsPlacementFactory
    {
        public IShipsPlacement GetShipsPlacement(IBattleshipsPlayer player)
        {
            return new ShipsPlacement(player, new ShipFactory());
        }
    }
}
