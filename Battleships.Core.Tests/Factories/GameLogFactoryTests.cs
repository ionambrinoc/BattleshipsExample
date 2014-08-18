namespace Battleships.Core.Tests.Factories
{
    using Battleships.Core.Factories;
    using Battleships.Core.Models;
    using Battleships.Player.Interface;
    using Battleships.Runner.Models;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class GameLogFactoryTests
    {
        private GameLogFactory factory;
        private PlayerRecord playerOneRecord;
        private PlayerRecord playerTwoRecord;
        private DateTime earlierTime;
        private DateTime laterTime;
        private IShipPosition[] playerOnePositions;
        private IShipPosition[] playerTwoPositions;

        [SetUp]
        public void SetUp()
        {
            factory = new GameLogFactory();
            earlierTime = new DateTime(2001, 1, 1);
            laterTime = new DateTime(2002, 1, 1);

            playerOneRecord = A.Fake<PlayerRecord>();
            playerTwoRecord = A.Fake<PlayerRecord>();
        }

        [Test]
        public void Can_create_GameLogs()
        {
            // When
            factory.InitialiseGameLog(playerOneRecord, playerTwoRecord, earlierTime, playerOnePositions, playerTwoPositions);
            factory.AddGameEvent(laterTime, true, new GridSquare('A', 1), true);
            var result = factory.GetCompleteGame(true, ResultType.Default);

            // Then
            result.DetailedLog.First().SelectedTarget.Column.Should().Be(1);
            // TO COMPLETE
        }
    }
}