namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;

    public interface IShipPositionValidator
    {
        bool IsValid(IEnumerable<IShipPosition> shipPositions);
    }

    public class ShipPositionValidator : IShipPositionValidator
    {
        public bool IsValid(IEnumerable<IShipPosition> shipPositions)
        {
            return true;
        }
    }
}