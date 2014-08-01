namespace Battleships.Runner.Tests.Models
{
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Runner.Models;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class ShipsPlacementTests
    {
        private List<IShip> ships;
        private ShipsPlacement shipsPlacement;
        private IBattleshipsPlayer player;
        private IShipFactory shipFactory;

        [SetUp]
        public void SetUp()
        {
            player = A.Fake<IBattleshipsPlayer>();
            shipFactory = A.Fake<IShipFactory>();
        }

        [Test]
        public void Ships_placement_is_invalid_if_overlapping_ships_provided()
        {
            // Given
            ships = new List<IShip>();
            var overlappingShip = FakeShip('F', 6, 'F', 10);
            AddToStandardShips(overlappingShip);
            FakeCalls();

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as overlapping ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_adjacent_ships_provided()
        {
            var belowValid = TestBelow();
            belowValid.Should().BeFalse("Should be invalid as a ship was directly below another");

            var leftValid = TestLeft();
            leftValid.Should().BeFalse("Should be invalid as a ship was directly to the left of another");

            var rightValid = TestRight();
            rightValid.Should().BeFalse("Should be invalid as a ship was directly to the right of another");

            var aboveValid = TestAbove();
            aboveValid.Should().BeFalse("Should be invalid as a ship was directly above another");
        }

        [Test]
        public void Ships_placement_is_invalid_if_not_all_ships_provided()
        {
            // Given
            ships = new List<IShip>();
            var ship = FakeShip('A', 1, 'A', 5);
            AddToShips(ship);
            FakeCalls();

            // When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as not all required ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_too_many_ships_provided()
        {
            // Given
            ships = new List<IShip>();
            var shipOne = FakeShip('J', 1, 'J', 5);
            AddToStandardShips(shipOne);

            var shipTwo = FakeShip('H', 4, 'H', 8);
            AddToStandardShips(shipTwo);

            FakeCalls();

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as too many ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_ships_provided_with_wrong_combinations_of_lengths()
        {
            // Given
            ships = new List<IShip>();
            var shipOne = FakeShip('A', 1, 'D', 1);
            AddToShips(shipOne, true, 4);

            var shipTwo = FakeShip('A', 3, 'A', 4);
            AddToShips(shipTwo, true, 2);

            var shipThree = FakeShip('E', 9, 'G', 9);
            AddToShips(shipThree, true, 3);

            var shipFour = FakeShip('D', 5, 'D', 7);
            AddToShips(shipFour, true, 3);

            var shipFive = FakeShip('H', 5, 'H', 7);
            AddToShips(shipFive, true, 3);

            FakeCalls();

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as too many ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_invalid_ship_provided()
        {
            // Given
            ships = new List<IShip>();
            var invalidShip = FakeShip('J', 1, 'J', 6);
            AddToShips(invalidShip);
            ShipIsInvalid(invalidShip);

            FakeCalls();

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as an invalid ship was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_player_is_invalid()
        {
            // Given
            A.CallTo(() => player.GetShipPositions()).Throws(() => new Exception());
            shipsPlacement = new ShipsPlacement(player, shipFactory);

            // When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Ships placement should be invalid if the player throws an exception");
        }

        [Test]
        public void All_ships_are_hit_when_17_cells_hit()
        {
            // Given
            SetUp17GridCells();
            shipsPlacement = new ShipsPlacement(player, shipFactory);

            for (var i = 0; i < 17; i++)
            {
                shipsPlacement.IsHit(new GridSquare('A', i));
            }

            // When
            var allHit = shipsPlacement.AllHit();

            // Then
            allHit.Should().BeTrue("All ships should have been hit but were not");
        }

        [Test]
        public void Shot_on_horizontal_ship_is_hit()
        {
            // Given
            ships = new List<IShip>();
            var horizontalShip = FakeShip('E', 1, 'E', 3);
            AddToShips(horizontalShip);
            FakeCalls();

            var target = new GridSquare('E', 1);
            TargetHitsShip(target, horizontalShip);

            // When
            var isHit = shipsPlacement.IsHit(target);

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_on_vertical_ship_is_hit()
        {
            // Given
            ships = new List<IShip>();
            var verticalShip = FakeShip('A', 1, 'C', 1);
            AddToShips(verticalShip);
            FakeCalls();

            var target = new GridSquare('B', 1);
            TargetHitsShip(target, verticalShip);

            // When
            var isHit = shipsPlacement.IsHit(target);

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_not_on_ship_is_miss()
        {
            // Given
            ships = new List<IShip>();
            var ship = FakeShip('G', 1, 'G', 3);
            AddToShips(ship);
            FakeCalls();

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('C', 1));

            // Then
            isHit.Should().BeFalse("Ship should have been missed but was hit");
        }

        [Test]
        public void Shot_in_the_middle_of_ship_hits()
        {
            // Given
            ships = new List<IShip>();
            var ship = FakeShip('C', 1, 'C', 3);
            AddToShips(ship);
            FakeCalls();

            var target = new GridSquare('C', 2);
            TargetHitsShip(target, ship);

            // When
            var isHit = shipsPlacement.IsHit(target);

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void If_set_already_contains_cell_it_is_not_readded()
        {
            // Given
            ships = new List<IShip>();
            var ship = FakeShip('A', 1, 'A', 3);
            AddToShips(ship);
            FakeCalls();

            for (var i = 0; i < 17; i++)
            {
                shipsPlacement.IsHit(new GridSquare('A', 1));
            }

            // When
            var allHit = shipsPlacement.AllHit();

            // Then
            allHit.Should().BeFalse("Hits on the same cell should be stored once but were not");
        }

        private bool TestBelow()
        {
            ships = new List<IShip>();
            var adjacentBelowShip = FakeShip('H', 6, 'H', 10);
            AddToStandardShips(adjacentBelowShip);
            FakeCalls();
            return shipsPlacement.IsValid();
        }

        private bool TestLeft()
        {
            ships = new List<IShip>();
            var adjacentLeftOfShip = FakeShip('G', 4, 'G', 8);
            AddToStandardShips(adjacentLeftOfShip);
            FakeCalls();
            return shipsPlacement.IsValid();
        }

        private bool TestRight()
        {
            ships = new List<IShip>();
            var adjacentRightOfShip = FakeShip('D', 10, 'H', 10);
            AddToStandardShips(adjacentRightOfShip);
            FakeCalls();
            return shipsPlacement.IsValid();
        }

        private bool TestAbove()
        {
            ships = new List<IShip>();
            var adjacentAboveShip = FakeShip('A', 9, 'D', 9);
            AddToStandardShips(adjacentAboveShip);
            FakeCalls();
            return shipsPlacement.IsValid();
        }

        private void TargetHitsShip(IGridSquare target, IShip ship)
        {
            A.CallTo(() => ship.IsTargetInShip(target)).Returns(true);
        }

        private void SetUp17GridCells()
        {
            ships = new List<IShip>();
            for (var i = 0; i < 17; i++)
            {
                var column = i;
                var ship = FakeShip('A', column, 'B', column);
                AddToShips(ship, true, 2);
                A.CallTo(() => ship.IsTargetInShip(new GridSquare('A', column))).Returns(true);
            }

            FakeCalls();
        }

        private void AddToStandardShips(IShip ship, bool valid = true, int length = 5)
        {
            AddToShips(ship, valid, length);

            var shipOne = FakeShip('A', 1, 'D', 1);
            AddToShips(shipOne, true, 4);

            var shipTwo = FakeShip('A', 3, 'A', 4);
            AddToShips(shipTwo, true, 2);

            var shipThree = FakeShip('E', 9, 'G', 9);
            AddToShips(shipThree, true, 3);

            var shipFour = FakeShip('D', 5, 'D', 7);
            AddToShips(shipFour, true, 3);
        }

        private void FakeCalls()
        {
            var shipPos = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => player.GetShipPositions()).Returns(shipPos);

            A.CallTo(() => shipFactory.GetShips(shipPos)).Returns(ships);
            shipsPlacement = new ShipsPlacement(player, shipFactory);
        }

        private void AddToShips(IShip ship, bool valid = true, int length = 5)
        {
            SetShipValidity(ship, valid);
            ShipIsOfLength(ship, length);
            ships.Add(ship);
        }

        private IShip FakeShip(char startRow, int startColumn, char endRow, int endColumn)
        {
            var ship = A.Fake<IShip>();
            A.CallTo(() => ship.StartingSquare).Returns(new GridSquare(startRow, startColumn));
            A.CallTo(() => ship.EndingSquare).Returns(new GridSquare(endRow, endColumn));

            return ship;
        }

        private void ShipIsInvalid(IShip ship)
        {
            SetShipValidity(ship, false);
        }

        private void SetShipValidity(IShip ship, bool valid)
        {
            A.CallTo(() => ship.IsValid).Returns(valid);
        }

        private void ShipIsOfLength(IShip ship, int length)
        {
            A.CallTo(() => ship.ShipLength).Returns(length);
        }
    }
}
