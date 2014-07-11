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

        [SetUp]
        public void SetUp()
        {
            player = A.Fake<IBattleshipsPlayer>();
        }

        [Test]
        public void Ships_placement_is_invalid_if_not_all_ships_provided()
        {
            // Given
            ThereIsAShipAt('A', 1, 'B', 1);

            // When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as not all required ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_too_many_ships_provided()
        {
            // Given
            var shipPositions = new List<IShipPosition>
                                {
                                    new ShipPosition(new GridSquare('J', 1), new GridSquare('J', 5)),
                                    new ShipPosition(new GridSquare('G', 8), new GridSquare('H', 8)),
                                };
            ThereAreShipsAt(shipPositions);

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as too many ships were provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_ship_that_is_off_the_board_is_provided()
        {
            // Given
            var shipPositions = new List<IShipPosition>
                                {
                                    new ShipPosition(new GridSquare('J', 0), new GridSquare('J', 4))
                                };
            ThereAreShipsAt(shipPositions);

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as a ship that is off the board was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_wrong_size_ship_provided()
        {
            // Given
            var shipPositions = new List<IShipPosition>
                                {
                                    new ShipPosition(new GridSquare('J', 1), new GridSquare('J', 6))
                                };
            ThereAreShipsAt(shipPositions);

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as a ship of the wrong size was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_diagonal_ship_provided()
        {
            // Given
            var shipPositions = new List<IShipPosition>
                                {
                                    new ShipPosition(new GridSquare('F', 1), new GridSquare('J', 5))
                                };
            ThereAreShipsAt(shipPositions);

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as a diagonal ship was provided");
        }

        [Test]
        public void Ships_placement_is_invalid_if_adjacent_ships()
        {
            // Given
            var shipPositions = new List<IShipPosition>
                                {
                                    new ShipPosition(new GridSquare('J', 1), new GridSquare('E', 1))
                                };
            ThereAreShipsAt(shipPositions);

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeFalse("Should be invalid as adjacent ships were provided");
        }

        [Test]
        public void Ships_with_reverse_orientation_are_still_valid()
        {
            // Given
            var shipPositions = new List<IShipPosition>
                                {
                                    new ShipPosition(new GridSquare('J', 5), new GridSquare('J', 1))
                                };
            ThereAreShipsAt(shipPositions);

            //When
            var valid = shipsPlacement.IsValid();

            // Then
            valid.Should().BeTrue("Should be valid as orientation of ships doesn't matter");
        }

        [Test]
        public void Ships_placement_is_invalid_if_player_is_invalid()
        {
            // Given
            A.CallTo(() => player.GetShipPositions()).Throws(() => new Exception());
            shipsPlacement = new ShipsPlacement(player);

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
            shipsPlacement = new ShipsPlacement(player);

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
            ThereIsAShipAt('E', 1, 'E', 3);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('E', 1));

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_on_vertical_ship_is_hit()
        {
            // Given
            ThereIsAShipAt('A', 1, 'B', 1);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('B', 1));

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void Shot_not_on_ship_is_miss()
        {
            // Given
            ThereIsAShipAt('G', 1, 'G', 5);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('C', 1));

            // Then
            isHit.Should().BeFalse("Ship should have been missed but was hit");
        }

        [Test]
        public void Shot_off_the_board_misses()
        {
            // Given
            ThereIsAShipAt('G', 1, 'G', 5);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('A', 100));

            // Then
            isHit.Should().BeFalse("Shot off the board should return false");
        }

        [Test]
        public void Shot_in_the_middle_of_ship_hits()
        {
            // Given
            ThereIsAShipAt('C', 1, 'C', 5);

            // When
            var isHit = shipsPlacement.IsHit(new GridSquare('C', 3));

            // Then
            isHit.Should().BeTrue("Ship should have been hit but was not");
        }

        [Test]
        public void If_set_already_contains_cell_it_is_not_readded()
        {
            // Given
            ThereIsAShipAt('A', 1, 'B', 1);
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
            var listOfShips = new List<IShipPosition>();
            for (var i = 0; i < 17; i++)
            {
                listOfShips.Add(new ShipPosition(new GridSquare('A', i), new GridSquare('B', i)));
            }

            A.CallTo(() => player.GetShipPositions()).Returns(listOfShips);
        }

        private void ThereIsAShipAt(char startSquareRow, int startSquareColumn, char endSquareRow, int endSquareColumn)
        {
            A.CallTo(() => player.GetShipPositions()).Returns(new List<IShipPosition>
                                                              {
                                                                  new ShipPosition(new GridSquare(startSquareRow, startSquareColumn),
                                                                      new GridSquare(endSquareRow, endSquareColumn))
                                                              });
            shipsPlacement = new ShipsPlacement(player);
        }

        private void ThereAreShipsAt(List<IShipPosition> shipPositions)
        {
            var constantShips = new List<IShipPosition>
                                {
                                    new ShipPosition(new GridSquare('A', 1), new GridSquare('D', 1)),
                                    new ShipPosition(new GridSquare('A', 3), new GridSquare('A', 4)),
                                    new ShipPosition(new GridSquare('D', 9), new GridSquare('F', 9)),
                                    new ShipPosition(new GridSquare('D', 5), new GridSquare('D', 7))
                                };
            shipPositions.AddRange(constantShips);

            A.CallTo(() => player.GetShipPositions()).Returns(shipPositions);
            shipsPlacement = new ShipsPlacement(player);
        }
    }
}