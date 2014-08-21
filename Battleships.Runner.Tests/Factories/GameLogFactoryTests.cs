namespace Battleships.Runner.Tests.Factories
{
    using Battleships.Core.Models;
    using Battleships.Player.Interface;
    using Battleships.Runner.Factories;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class GameLogFactoryTests
    {
        private const string ValidShipPosition1String = "A1A2";
        private const string ValidShipPosition2String = "A3A4";
        private const string ValidGridSquareString = "A1";
        private readonly ShipPosition validShipPosition1 = new ShipPosition(new GridSquare('A', 1), new GridSquare('A', 2));
        private readonly ShipPosition validShipPosition2 = new ShipPosition(new GridSquare('A', 3), new GridSquare('A', 4));
        private readonly GridSquare validGridSquare = new GridSquare('A', 1);
        private GameLogFactory factory;
        private PlayerRecord playerOneRecord;
        private PlayerRecord playerTwoRecord;
        private DateTime earlierTime;
        private DateTime laterTime;
        private List<ShipPosition> player1Positions;
        private List<ShipPosition> player2Positions;

        [SetUp]
        public void SetUp()
        {
            factory = new GameLogFactory(new GridSquareStringConverter());
            earlierTime = new DateTime(2001, 1, 1);
            laterTime = new DateTime(2002, 1, 1);

            playerOneRecord = A.Fake<PlayerRecord>();
            playerTwoRecord = A.Fake<PlayerRecord>();

            player1Positions = new List<ShipPosition> { validShipPosition1 };
            player2Positions = new List<ShipPosition> { validShipPosition2 };
        }

        [Test]
        public void Can_create_GameLogs()
        {
            // When
            factory.InitialiseGameLog(playerOneRecord, playerTwoRecord, earlierTime, player1Positions, player2Positions);
            factory.AddGameEvent(laterTime, true, validGridSquare, true);
            var result = factory.GetCompleteGame(true, ResultType.Default);

            // Then
            result.Should().NotBe(null);
        }

        [Test]
        public void Can_set_game_log_fields()
        {
            // When
            factory.InitialiseGameLog(playerOneRecord, playerTwoRecord, earlierTime, player1Positions, player2Positions);
            factory.AddGameEvent(laterTime, true, validGridSquare, true);
            var result = factory.GetCompleteGame(true, ResultType.Default);

            // Then
            result.Player1.Should().Be(playerOneRecord);
            result.Player2.Should().Be(playerTwoRecord);
            result.StartTime.Should().Be(earlierTime);
            result.Player1PositionsString.Should().Be(ValidShipPosition1String);
            result.Player2PositionsString.Should().Be(ValidShipPosition2String);
            result.DetailedLog.First().Time.Should().Be(laterTime);
            result.DetailedLog.First().IsPlayer1Turn.Should().Be(true);
            result.DetailedLog.First().SelectedTarget.Should().Be(ValidGridSquareString);
            result.DetailedLog.First().IsHit.Should().Be(true);
            result.Player1Won.Should().Be(true);
            result.ResultType.Should().Be(ResultType.Default);
        }
    }
}