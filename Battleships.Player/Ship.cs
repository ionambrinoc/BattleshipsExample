namespace Battleships.Player
{
    public interface IShip
    {
        bool IsHorizontal { get; }
        int ShipLength { get; }
        IGridSquare StartingSquare { get; }
        IGridSquare EndingSquare { get; }
        bool IsValid { get; }
    }

    public class Ship : IShip
    {
        private readonly IShipPosition shipPosition;

        public Ship(IShipPosition shipPosition)
        {
            this.shipPosition = shipPosition;

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

        public IGridSquare StartingSquare
        {
            get
            {
                if (shipPosition.StartingSquare.Column <= shipPosition.EndingSquare.Column && shipPosition.StartingSquare.Row <= shipPosition.EndingSquare.Row)
                {
                    return shipPosition.StartingSquare;
                }
                return shipPosition.EndingSquare;
            }
        }

        public IGridSquare EndingSquare
        {
            get
            {
                if (shipPosition.EndingSquare.Column <= shipPosition.StartingSquare.Column && shipPosition.EndingSquare.Row <= shipPosition.StartingSquare.Row)
                {
                    return shipPosition.EndingSquare;
                }
                return shipPosition.StartingSquare;
            }
        }

        public bool IsValid { get; private set; }

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