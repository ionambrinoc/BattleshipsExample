namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using System.Data.Entity;

    public class BattleshipsContext : DbContext
    {
        public BattleshipsContext() : base("DefaultConnection") {}

        public BattleshipsContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<Kitten> Kittens { get; set; }

        public DbSet<Player> Bots { get; set; }

        public DbSet<GameResult> GameResults { get; set; }
    }
}