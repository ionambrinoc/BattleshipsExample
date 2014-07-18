namespace Battleships.ExamplePlayer
{
    using Battleships.Player;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ExamplePlayerTests
    {
        private ExamplePlayer player;

        [Test]
        public void First_square_is_A_1()
        {
            player = new ExamplePlayer();

            var firstTarget = player.SelectTarget();

            firstTarget.Should().Be(new GridSquare('A', 1));
        }

        [Test]
        public void Second_square_is_A_2()
        {
            player = new ExamplePlayer();

            player.SelectTarget();
            var secondTarget = player.SelectTarget();

            secondTarget.Should().Be(new GridSquare('A', 2));
        }

        [Test]
        public void Selects_next_row_when_reaching_end_of_row()
        {
            player = new ExamplePlayer { LastTarget = new GridSquare('A', 10) };

            var secondTarget = player.SelectTarget();

            secondTarget.Should().Be(new GridSquare('B', 1));
        }
    }
}
