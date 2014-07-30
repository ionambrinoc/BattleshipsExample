namespace Battleships.Player.Interface
{
    public sealed class GridSquare : IGridSquare
    {
        public GridSquare(char row, int column)
        {
            Row = row;
            Column = column;
        }

        public char Row { get; private set; }
        public int Column { get; private set; }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row.GetHashCode() * 397) ^ Column;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((GridSquare)obj);
        }

        public bool Equals(GridSquare gridSquare)
        {
            return (gridSquare.Column == Column) && (gridSquare.Row == Row);
        }
    }
}
