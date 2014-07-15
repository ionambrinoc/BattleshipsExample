namespace Battleships.Player
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IShipFactory
    {
        List<IShip> GetShips(IEnumerable<IShipPosition> shipPosition);
    }

    public class ShipFactory : IShipFactory
    {
        public List<IShip> GetShips(IEnumerable<IShipPosition> shipPositions)
        {
            return shipPositions.Select(ship => new Ship(ship)).Cast<IShip>().ToList();
        }
    }
}