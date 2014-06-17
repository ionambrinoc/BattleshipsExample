namespace Battleships.Player
{
    public class GridSquare : IGridSquare
    {
        public GridSquare(char row, int column)
        {
            Row = row;
            Column = column;
        }

        public char Row { get; private set; }
        public int Column { get; private set; }
    }
}