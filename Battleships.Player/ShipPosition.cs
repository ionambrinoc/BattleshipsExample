namespace Battleships.Player
{
    public class ShipPosition : IShipPosition
    {
        public ShipPosition(IGridSquare startingSquare, IGridSquare endingSquare)
        {
            StartingSquare = startingSquare;
            EndingSquare = endingSquare;
        }

        public IGridSquare StartingSquare { get; private set; }
        public IGridSquare EndingSquare { get; private set; }
    }
}