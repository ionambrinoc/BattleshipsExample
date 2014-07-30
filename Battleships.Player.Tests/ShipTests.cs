namespace Battleships.Player.Tests
{
    using Battleships.Player.Interface;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;

    internal class ShipTests
    {
        private IShip ship;
        private IShipPosition shipPosition;

        [SetUp]
        public void SetUp()
        {
            shipPosition = A.Fake<IShipPosition>();
        }

        [Test]
        public void Diagonal_ship_is_not_valid()
        {
            // Given
            ShipIsAt('A', 1, 'B', 2);

            // When
            ship = new Ship(shipPosition);
            var valid = ship.IsValid;

            // Then
            valid.Should().BeFalse("because ship is diagonal");
        }

        [Test]
        public void Ship_outside_the_grid_is_not_valid()
        {
            // When
            ShipIsAt('A', 0, 'A', 1);
            var leftOfBoardValid = IsShipValid();

            ShipIsAt('d', 3, 'd', 5);
            var aboveBoardValid = IsShipValid();

            ShipIsAt('D', 100, 'D', 104);
            var rightOfBoardValid = IsShipValid();

            ShipIsAt('J', 5, 'M', 5);
            var belowBoardValid = IsShipValid();

            // Then
            leftOfBoardValid.Should().BeFalse("because ship is left of the grid");
            aboveBoardValid.Should().BeFalse("because ship is above the grid");
            rightOfBoardValid.Should().BeFalse("because ship is right of the grid");
            belowBoardValid.Should().BeFalse("because ship is below the grid");
        }

        [Test]
        public void Ship_of_too_small_size_is_not_valid()
        {
            // Given
            ShipIsAt('A', 1, 'A', 1);

            // When
            ship = new Ship(shipPosition);
            var valid = ship.IsValid;

            // Then
            valid.Should().BeFalse("because ship is of length 1");
        }

        [Test]
        public void Ship_of_too_large_size_is_not_valid()
        {
            // Given
            ShipIsAt('A', 1, 'A', 6);

            // When
            ship = new Ship(shipPosition);
            var valid = ship.IsValid;

            // Then
            valid.Should().BeFalse("because ship is of length 6");
        }

        [Test]
        public void Horizontal_ship_is_horizontal()
        {
            // Given
            ShipIsAt('A', 1, 'A', 2);

            // When
            ship = new Ship(shipPosition);
            var horizontal = ship.IsHorizontal;

            // Then
            horizontal.Should().BeTrue("because the ship is horizontal");
        }

        [Test]
        public void Vertical_ship_is_not_horizontal()
        {
            // Given
            ShipIsAt('A', 1, 'B', 1);

            // When
            ship = new Ship(shipPosition);
            var horizontal = ship.IsHorizontal;

            // Then
            horizontal.Should().BeFalse("because the ship is vertical");
        }

        [Test]
        public void ShipLength_returns_correct_value()
        {
            // Given
            ShipIsAt('A', 1, 'C', 1);

            // When
            ship = new Ship(shipPosition);
            var length = ship.ShipLength;

            // Then
            length.Should().Be(3);
        }

        [Test]
        public void Ship_coordinates_are_not_swapped_if_orientated_correctly()
        {
            // Given
            ShipIsAt('A', 1, 'B', 1);

            // When
            ship = new Ship(shipPosition);
            var startingSquare = ship.StartingSquare;
            var endingSquare = ship.EndingSquare;

            // Then
            startingSquare.ShouldBeEquivalentTo(new GridSquare('A', 1));
            endingSquare.ShouldBeEquivalentTo(new GridSquare('B', 1));
        }

        [Test]
        public void Ship_coordinates_are_swapped_if_orientated_incorrectly()
        {
            // Given
            ShipIsAt('B', 1, 'A', 1);

            // When
            ship = new Ship(shipPosition);
            var startingSquare = ship.StartingSquare;
            var endingSquare = ship.EndingSquare;

            // Then
            startingSquare.ShouldBeEquivalentTo(new GridSquare('A', 1));
            endingSquare.ShouldBeEquivalentTo(new GridSquare('B', 1));
        }

        [Test]
        public void Target_is_in_ship_if_between_StartingSquare_and_EndingSquare()
        {
            // Given
            ShipIsAt('A', 1, 'C', 1);
            var target = new GridSquare('B', 1);

            // When
            ship = new Ship(shipPosition);
            var isInShip = ship.IsTargetInShip(target);

            // Then
            isInShip.Should().BeTrue("because the target was between StartingSquare and EndingSquare");
        }

        [Test]
        public void Target_is_not_in_ship_if_not_between_StartingSquare_and_EndingSquare()
        {
            // Given
            ShipIsAt('A', 1, 'C', 1);
            var target = new GridSquare('D', 1);

            // When
            ship = new Ship(shipPosition);
            var isInShip = ship.IsTargetInShip(target);

            // Then
            isInShip.Should().BeFalse("because the target was not between StartingSquare and EndingSquare");
        }

        private bool IsShipValid()
        {
            ship = new Ship(shipPosition);
            return ship.IsValid;
        }

        private void ShipIsAt(char startRow, int startCol, char endRow, int endCol)
        {
            A.CallTo(() => shipPosition.StartingSquare).Returns(new GridSquare(startRow, startCol));
            A.CallTo(() => shipPosition.EndingSquare).Returns(new GridSquare(endRow, endCol));
        }
    }
}
