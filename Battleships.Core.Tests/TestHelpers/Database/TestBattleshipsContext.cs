namespace Battleships.Core.Tests.TestHelpers.Database
{
    using Battleships.Core;
    using System.Data.Entity;

    public class TestBattleshipsContext : BattleshipsContext
    {
        public TestBattleshipsContext() {}

        public TestBattleshipsContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}