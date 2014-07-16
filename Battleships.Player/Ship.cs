namespace Battleships.Player
{
    public interface IShip
    {
        bool IsHorizontal { get; }
        int ShipLength { get; }
        IGridSquare StartingSquare { get; }
        IGridSquare EndingSquare { get; }
        bool IsValid { get; }
        bool IsTargetInShip(IGridSquare target);
    }

    public class Ship : IShip
    {
        public Ship(IShipPosition shipPosition)
        {
            OrientateShipCorrectly(shipPosition);

            if (!IsDiagonal() && !IsOutsideGrid() && ShipLength < 6 && ShipLength > 1)
            {
                IsValid = true;
            }
        }

        public bool IsHorizontal
        {
            get { return StartingSquare.Row == EndingSquare.Row; }
        }

        public int ShipLength
        {
            get { return (EndingSquare.Column - StartingSquare.Column) + (EndingSquare.Row - StartingSquare.Row) + 1; }
        }

        public IGridSquare StartingSquare { get; set; }
        public IGridSquare EndingSquare { get; set; }
        public bool IsValid { get; private set; }

        public bool IsTargetInShip(IGridSquare target)
        {
            if (IsHorizontal && StartingSquare.Row == target.Row)
            {
                return IsInRange(target.Column, EndingSquare.Column, StartingSquare.Column);
            }

            return StartingSquare.Column == target.Column && IsInRange(target.Row, EndingSquare.Row, StartingSquare.Row);
        }

        private bool IsInRange(int target, int shipEnd, int shipStart)
        {
            return (target <= shipEnd && target >= shipStart) || (target >= shipEnd && target <= shipStart);
        }

        private void OrientateShipCorrectly(IShipPosition shipPosition)
        {
            if (shipPosition.StartingSquare.Column <= shipPosition.EndingSquare.Column || shipPosition.StartingSquare.Row <= shipPosition.EndingSquare.Row)
            {
                StartingSquare = shipPosition.StartingSquare;
                EndingSquare = shipPosition.EndingSquare;
            }
            else
            {
                StartingSquare = shipPosition.EndingSquare;
                EndingSquare = shipPosition.StartingSquare;
            }
        }

        private bool IsDiagonal()
        {
            return (StartingSquare.Column != EndingSquare.Column) && !IsHorizontal;
        }

        private bool IsOutsideGrid()
        {
            return StartingSquare.Column < 1 || StartingSquare.Column > 10 || StartingSquare.Row < 'A' || StartingSquare.Row > 'J'
                   || EndingSquare.Column < 1 || EndingSquare.Column > 10 || EndingSquare.Row < 'A' || EndingSquare.Row > 'J';
        }
    }
}