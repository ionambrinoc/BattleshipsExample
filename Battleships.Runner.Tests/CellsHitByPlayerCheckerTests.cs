namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using FluentAssertions;
    using NUnit.Framework;

    public class CellsHitByPlayerCheckerTests
    {
        private CellsHitByPlayerChecker cellsHitByPlayerChecker;

        [SetUp]
        public void SetUp()
        {
            cellsHitByPlayerChecker = new CellsHitByPlayerChecker();
        }

        [Test]
        public void If_list_already_contains_cell_it_is_not_readded()
        {
            // Given
            cellsHitByPlayerChecker.AddCell(new GridSquare('A', 1));
            cellsHitByPlayerChecker.AddCell(new GridSquare('A', 1));

            // When
            var numberOfCells = cellsHitByPlayerChecker.Size;

            // Then
            numberOfCells.Should().Be(1);
        }

        [Test]
        public void AllHit_returns_true_after_all_ship_cells_hit()
        {
            // Given
            for (var i = 0; i < 17; i++)
            {
                cellsHitByPlayerChecker.AddCell(new GridSquare('A', i));
            }

            // When
            var allHit = cellsHitByPlayerChecker.AllHit();

            // Then
            allHit.Should().BeTrue();
        }
    }
}