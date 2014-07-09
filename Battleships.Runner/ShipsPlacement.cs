namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;

    public interface IShipsPlacement
    {
        bool IsValid();
        IEnumerable<IShipPosition> GetShipPositions();
    }

    public class ShipsPlacement : IShipsPlacement
    {
        private readonly IEnumerable<IShipPosition> positions;
        private readonly IBattleshipsPlayer player;

        public ShipsPlacement(IBattleshipsPlayer player)
        {
            this.player = player;
            positions = player.GetShipPositions();
        }

        public bool IsValid()
        {
            return true;
        }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            return positions;
        }
    }
}