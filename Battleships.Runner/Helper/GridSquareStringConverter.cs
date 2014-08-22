namespace Battleships.Runner.Helper
{
    using Battleships.Player.Interface;
    using System.Globalization;

    public class GridSquareStringConverter
    {
        public string ShipPositionToString(IShipPosition shipPosition)
        {
            return GridSquareToString(shipPosition.StartingSquare) + GridSquareToString(shipPosition.EndingSquare);
        }

        public string GridSquareToString(IGridSquare gridSquare)
        {
            return gridSquare.Row + gridSquare.Column.ToString(CultureInfo.CurrentCulture);
        }
    }
}