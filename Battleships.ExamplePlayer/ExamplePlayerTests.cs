namespace Battleships.ExamplePlayer
{
    using Battleships.Player.Interface;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ExamplePlayerTests
    {
        private ExamplePlayer player;

        [SetUp]
        public void SetUp()
        {
            player = new ExamplePlayer();
        }

        [Test]
        public void First_square_is_A_1()
        {
            // When
            var firstTarget = player.SelectTarget();

            // Then
            firstTarget.Should().Be(new GridSquare('A', 1));
        }

        [Test]
        public void Seventeen_ship_tiles_placed()
        {
            // When
            player.GetShipPositions();

            // Then
            player.CountGridSquares().Should().Be(17);
        }

        [Test]
        public void Second_square_is_A_2()
        {
            // Given
            player.SelectTarget();

            // When
            var secondTarget = player.SelectTarget();

            // Then
            secondTarget.Should().Be(new GridSquare('A', 2));
        }

        [Test]
        public void Selects_next_row_when_reaching_end_of_row()
        {
            // Given
            player.LastTarget = new GridSquare('A', 10);

            // When
            var secondTarget = player.SelectTarget();

            // Then
            secondTarget.Should().Be(new GridSquare('B', 1));
        }

        [Test]
        public void Selects_A_1_when_reaching_end_of_grid()
        {
            // Given
            player.LastTarget = new GridSquare('J', 10);

            // When
            var secondTarget = player.SelectTarget();

            // Then
            secondTarget.Should().Be(new GridSquare('A', 1));
        }
    }
}