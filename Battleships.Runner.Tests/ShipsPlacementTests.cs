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
            ShipIsValid(ship, true);
            ShipIsHorizontal(ship, true);
            ShipIsOfLength(ship, 5);
            ThereAreShipsAt(new List<IShip> { ship });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as overlapping ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_not_all_ships_provided()
        {
            // Given
            var ship = FakeShip('A', 1, 'C', 1);
            ShipIsValid(ship, true);
            ShipIsHorizontal(ship, false);
            ShipIsOfLength(ship, 5);
            ThereIsAShipAt(ship);

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
            ShipIsValid(shipOne, true);
            ShipIsHorizontal(shipOne, true);
            ShipIsOfLength(shipOne, 5);

            var shipTwo = FakeShip('H', 4, 'H', 8);
            ShipIsValid(shipTwo, true);
            ShipIsHorizontal(shipTwo, true);
            ShipIsOfLength(shipTwo, 5);

            ThereAreShipsAt(new List<IShip> { shipOne, shipTwo });

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
            ShipIsValid(ship, false);
            ShipIsHorizontal(ship, true);
            ShipIsOfLength(ship, 5);
            ThereAreShipsAt(new List<IShip> { ship });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as a ship that is off the board was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_wrong_size_ship_provided()
        {
            // Given
            var ship = FakeShip('J', 1, 'J', 6);
            ShipIsValid(ship, false);
            ShipIsHorizontal(ship, true);
            ShipIsOfLength(ship, 6);
            ThereAreShipsAt(new List<IShip> { ship });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as a ship of the wrong size was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_diagonal_ship_provided()
        {
            // Given
            var ship = FakeShip('F', 1, 'J', 5);
            ShipIsValid(ship, false);
            ShipIsHorizontal(ship, false);
            ShipIsOfLength(ship, 5);
            ThereAreShipsAt(new List<IShip> { ship });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as a diagonal ship was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_adjacent_ships()
        {
            // Given
            var ship = FakeShip('E', 1, 'I', 1);
            ShipIsValid(ship, true);
            ShipIsHorizontal(ship, false);
            ShipIsOfLength(ship, 5);
            ThereAreShipsAt(new List<IShip> { ship });

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as adjacent ships were provided");
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
            ThereAre17Ships();
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
            ShipIsHorizontal(ship, true);
            ThereIsAShipAt(ship);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('E', 1));

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_on_vertical_ship_is_hit()
        {
            // Given
            var ship = FakeShip('A', 1, 'C', 1);
            ShipIsHorizontal(ship, false);
            ThereIsAShipAt(ship);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('B', 1));

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_not_on_ship_is_miss()
        {
            // Given
            var ship = FakeShip('G', 1, 'G', 3);
            ShipIsHorizontal(ship, true);
            ThereIsAShipAt(ship);

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
            ShipIsHorizontal(ship, true);
            ThereIsAShipAt(ship);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('C', 2));

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void If_set_already_contains_cell_it_is_not_readded()
        {
            // Given
            var ship = FakeShip('A', 1, 'C', 1);
            ShipIsHorizontal(ship, false);
            ThereIsAShipAt(ship);

            for (var i = 0; i < 17; i++)
            {
                shipsPlacement.IsHit(new GridSquare('A', 1));
            }

            // When
            var allHit = shipsPlacement.AllHit();

            // Then
            allHit.Should().BeFalse("Hits on the same cell should be stored once but were not");
        }

        private void ThereAre17Ships()
        {
            var ships = new List<IShip>();
            for (var i = 0; i < 17; i++)
            {
                var ship = FakeShip('A', i, 'B', i);
                ships.Add(ship);
                SetUpShip(ship, true, false, 2);
            }

            var shipPos = A.CollectionOfFake<IShipPosition>(17);
            A.CallTo(() => player.GetShipPositions()).Returns(shipPos);

            A.CallTo(() => shipFactory.GetShips(shipPos)).Returns(ships);
        }

        private void ThereIsAShipAt(IShip ship)
        {
            var listOfShips = new List<IShip> { ship };

            var shipPos = A.CollectionOfFake<IShipPosition>(1);
            A.CallTo(() => player.GetShipPositions()).Returns(shipPos);

            A.CallTo(() => shipFactory.GetShips(shipPos)).Returns(listOfShips);
            shipsPlacement = new ShipsPlacement(player, shipFactory);
        }

        private void ThereAreShipsAt(List<IShip> ship)
        {
            var shipOne = FakeShip('A', 1, 'D', 1);
            SetUpShip(shipOne, true, false, 4);

            var shipTwo = FakeShip('A', 3, 'A', 4);
            SetUpShip(shipTwo, true, true, 2);

            var shipThree = FakeShip('D', 9, 'F', 9);
            SetUpShip(shipThree, true, false, 3);

            var shipFour = FakeShip('D', 5, 'D', 7);
            SetUpShip(shipFour, true, true, 3);

            var listOfShips = new List<IShip> { shipOne, shipTwo, shipThree, shipFour };
            listOfShips.AddRange(ship);

            var shipPos = A.CollectionOfFake<IShipPosition>(5);
            A.CallTo(() => player.GetShipPositions()).Returns(shipPos);

            A.CallTo(() => shipFactory.GetShips(shipPos)).Returns(listOfShips);
            shipsPlacement = new ShipsPlacement(player, shipFactory);
        }

        private void SetUpShip(IShip ship, bool valid, bool horizontal, int length)
        {
            ShipIsValid(ship, valid);
            ShipIsHorizontal(ship, horizontal);
            ShipIsOfLength(ship, length);
        }

        private IShip FakeShip(char startRow, int startColumn, char endRow, int endColumn)
        {
            var ship = A.Fake<IShip>();
            A.CallTo(() => ship.StartingSquare).Returns(new GridSquare(startRow, startColumn));
            A.CallTo(() => ship.EndingSquare).Returns(new GridSquare(endRow, endColumn));

            return ship;
        }

        private void ShipIsValid(IShip ship, bool valid)
        {
            A.CallTo(() => ship.IsValid).Returns(valid);
        }

        private void ShipIsHorizontal(IShip ship, bool horizontal)
        {
            A.CallTo(() => ship.IsHorizontal).Returns(horizontal);
        }

        private void ShipIsOfLength(IShip ship, int length)
        {
            A.CallTo(() => ship.ShipLength).Returns(length);
        }
    }
}