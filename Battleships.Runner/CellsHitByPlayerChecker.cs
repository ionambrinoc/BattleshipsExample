namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;

    public interface ICellsHitByPlayerChecker
    {
        bool AllHit();
        void AddCell(IGridSquare target);
    }

    public class CellsHitByPlayerChecker : ICellsHitByPlayerChecker
    {
        private readonly List<IGridSquare> cellsOfShipsHit;

        public CellsHitByPlayerChecker()
        {
            cellsOfShipsHit = new List<IGridSquare>();
        }

        public bool AllHit()
        {
            return cellsOfShipsHit.Count == 17;
        }

        public void AddCell(IGridSquare target)
        {
            if (!cellsOfShipsHit.Contains(target))
            {
                cellsOfShipsHit.Add(target);
            }
        }
    }
}