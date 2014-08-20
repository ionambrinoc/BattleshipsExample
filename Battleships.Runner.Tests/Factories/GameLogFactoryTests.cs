namespace Battleships.Runner.Tests.Factories
{
    using Battleships.Core.Models;
    using Battleships.Player.Interface;
    using Battleships.Runner.Factories;
    using FakeItEasy;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class GameLogFactoryTests
    {
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

            player1Positions = new List<ShipPosition> { (new ShipPosition(new GridSquare('A', 3), new GridSquare('A', 7))) };
            player2Positions = new List<ShipPosition> { (new ShipPosition(new GridSquare('A', 3), new GridSquare('A', 7))) };
        }

        [Test]
        public void Can_create_GameLogs()
        {
            // When
            //factory.InitialiseGameLog(playerOneRecord, playerTwoRecord, earlierTime, player1Positions, player2Positions);
            //factory.AddGameEvent(laterTime, true, new GridSquare('A', 1), true);
            //var result = factory.GetCompleteGame(true, ResultType.Default);

            // Then
            //result.DetailedLog.First().SelectedTarget.Column.Should().Be(1);
            // TO COMPLETE
        }
    }
}