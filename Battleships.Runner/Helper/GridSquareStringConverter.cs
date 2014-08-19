namespace Battleships.Runner.Factories
{
    using Battleships.Player.Interface;

    public class GridSquareStringConverter 
    {
        public string ShipPositionToString(IShipPosition shipPosition)
        {
            return GridSquareToString(shipPosition.StartingSquare) + GridSquareToString(shipPosition.EndingSquare);
        }

        public string GridSquareToString(IGridSquare gridSquare)
        {
            return gridSquare.Row + gridSquare.Column.ToString();
        }

    }
}