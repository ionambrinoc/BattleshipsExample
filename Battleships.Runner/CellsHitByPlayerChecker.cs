namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;

    public class CellsHitByPlayerChecker
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