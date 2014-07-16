namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    internal class ShipsPlacementTests
    {
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
            var ship = FakeShip('F', 6, 'F', 10);
            SetUpShip(ship);
            FakeCallsTo(new List<IShip> { ship });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as overlapping ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_not_all_ships_provided()
        {
            // Given
            var ship = FakeShip('A', 1, 'A', 3);
            SetUpShip(ship);
            FakeCallsTo(new List<IShip> { ship });

            // When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as not all required ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_too_many_ships_provided()
        {
            // Given
            var shipOne = FakeShip('J', 1, 'J', 5);
            SetUpShip(shipOne);

            var shipTwo = FakeShip('H', 4, 'H', 8);
            SetUpShip(shipTwo);

            FakeCallsTo(new List<IShip> { shipOne, shipTwo });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as too many ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_ship_that_is_off_the_board_is_provided()
        {
            // Given
            var ship = FakeShip('J', 0, 'J', 4);
            SetUpShip(ship);
            ShipIsInvalid(ship);

            FakeCallsTo(new List<IShip> { ship });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as a ship that is off the board was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_invalid_ship_provided()
        {
            // Given
            var ship = FakeShip('J', 1, 'J', 6);
            SetUpShip(ship);
            ShipIsInvalid(ship);

            FakeCallsTo(new List<IShip> { ship });

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
            var ship = FakeShip('E', 1, 'E', 3);
            FakeCallsTo(new List<IShip> { ship });

            var target = new GridSquare('E', 1);
            TargetHitsShip(target, ship);

            // When
            var isHit = shipsPlacement.IsHit(target);

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_on_vertical_ship_is_hit()
        {
            // Given
            var ship = FakeShip('A', 1, 'C', 1);
            FakeCallsTo(new List<IShip> { ship });

            var target = new GridSquare('B', 1);
            TargetHitsShip(target, ship);

            // When
            var isHit = shipsPlacement.IsHit(target);

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_not_on_ship_is_miss()
        {
            // Given
            var ship = FakeShip('G', 1, 'G', 3);
            FakeCallsTo(new List<IShip> { ship });

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('C', 1));

            // Then
            isHit.Should().BeFalse("Ship should have been missed but was hit");
        }

        [Test]
        public void Shot_in_the_middle_of_ship_hits()
        {
            // Given
            var ship = FakeShip('C', 1, 'C', 3);
            FakeCallsTo(new List<IShip> { ship });

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
            var ship = FakeShip('A', 1, 'A', 3);
            FakeCallsTo(new List<IShip> { ship });

            for (var i = 0; i < 17; i++)
            {
                shipsPlacement.IsHit(new GridSquare('A', 1));
            }

            // When
            var allHit = shipsPlacement.AllHit();

            // Then
            allHit.Should().BeFalse("Hits on the same cell should be stored once but were not");
        }

        private void TargetHitsShip(IGridSquare target, IShip ship)
        {
            A.CallTo(() => ship.IsTargetInShip(target)).Returns(true);
        }

        private void SetUp17GridCells()
        {
            var ships = new List<IShip>();
            for (var i = 0; i < 17; i++)
            {
                var column = i;
                var ship = FakeShip('A', column, 'B', column);
                ships.Add(ship);
                SetUpShip(ship, true, 2);
                A.CallTo(() => ship.IsTargetInShip(new GridSquare('A', column))).Returns(true);
            }

            var shipPos = A.CollectionOfFake<IShipPosition>(17);
            A.CallTo(() => player.GetShipPositions()).Returns(shipPos);

            A.CallTo(() => shipFactory.GetShips(shipPos)).Returns(ships);
        }

        private void FakeCallsTo(List<IShip> ship)
        {
            var shipOne = FakeShip('A', 1, 'D', 1);
            SetUpShip(shipOne, true, 4);

            var shipTwo = FakeShip('A', 3, 'A', 4);
            SetUpShip(shipTwo, true, 2);

            var shipThree = FakeShip('D', 9, 'F', 9);
            SetUpShip(shipThree, true, 3);

            var shipFour = FakeShip('D', 5, 'D', 7);
            SetUpShip(shipFour, true, 3);

            var listOfShips = new List<IShip> { shipOne, shipTwo, shipThree, shipFour };
            listOfShips.AddRange(ship);

            var shipPos = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => player.GetShipPositions()).Returns(shipPos);

            A.CallTo(() => shipFactory.GetShips(shipPos)).Returns(listOfShips);
            shipsPlacement = new ShipsPlacement(player, shipFactory);
        }

        private void SetUpShip(IShip ship, bool valid = true, int length = 5)
        {
            SetShipValidity(ship, valid);
            ShipIsOfLength(ship, length);
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