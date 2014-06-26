namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using System.Data.Entity;

    public class BattleshipsContext : DbContext
    {
        public BattleshipsContext() : base("DefaultConnection") {}

        public DbSet<Kitten> Kittens { get; set; }
    }
}
