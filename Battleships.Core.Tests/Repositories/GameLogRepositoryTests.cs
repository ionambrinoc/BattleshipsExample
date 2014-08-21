namespace Battleships.Core.Tests.Repositories
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Core.Tests.TestHelpers.Attributes;
    using Battleships.Core.Tests.TestHelpers.Database;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    [RequiresDatabase]
    public class GameLogRepositoryTests
    {
        private GameLogRepository repo;
        private GameLog gameLog;

        [SetUp]
        public void SetUp()
        {
            repo = new GameLogRepository(new TestBattleshipsContext(TestDb.ConnectionString));
            gameLog = new GameLog
                      {
                          StartTime = new DateTime(2000, 1, 1)
                      };
        }

        [Test]
        public void Will_store_game_logs()
        {
            // Given
            repo.AddGameLog(gameLog);
            repo.SaveContext();

            // When
            var result = repo.GetAll();

            // Then
            result.Count().Should().Be(1);
        }
    }
}