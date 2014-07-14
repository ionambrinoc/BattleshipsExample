namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using System.Data.Entity;

    public class BattleshipsContext : DbContext
    {
        public BattleshipsContext() : base("DefaultConnection") {}

        public BattleshipsContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<PlayerRecord> PlayerRecords { get; set; }
    }
}