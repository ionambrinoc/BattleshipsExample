namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICellsHitByPlayerChecker
    {
        bool AllHit();
        void AddCell(IGridSquare target);
    }

    public class CellsHitByPlayerChecker : ICellsHitByPlayerChecker
    {
        public int Size;
        private const int TotalNumberOfShipCells = 17;
        private readonly List<IGridSquare> cellsOfShipsHit;

        public CellsHitByPlayerChecker()
        {
            cellsOfShipsHit = new List<IGridSquare>();
            Size = 0;
        }

        public bool AllHit()
        {
            return cellsOfShipsHit.Count == TotalNumberOfShipCells;
        }

        public void AddCell(IGridSquare target)
        {
            var matches = cellsOfShipsHit.Where(p => p.Equals(target));
            if (!matches.Any())
            {
                cellsOfShipsHit.Add(target);
                Size += 1;
            }
        }
    }
}