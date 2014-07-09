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

        public override bool Equals(object obj)
        {
            return Equals(obj as GridSquare);
        }

        public bool Equals(GridSquare gridSquare)
        {
            return (gridSquare.Column == Column) && (gridSquare.Row == Row);
        }
    }
}