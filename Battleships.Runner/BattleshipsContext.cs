namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class BattleshipsContext : IdentityDbContext<User>
    {
        public BattleshipsContext() : base("DefaultConnection") {}

        public BattleshipsContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<PlayerRecord> PlayerRecords { get; set; }

        public DbSet<GameResult> GameResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameResult>()
                        .HasRequired<PlayerRecord>(m => m.Winner)
                        .WithMany(t => t.WonGameResults)
                        .HasForeignKey(m => m.WinnerId)
                        .WillCascadeOnDelete(false);
            modelBuilder.Entity<GameResult>()
                        .HasRequired<PlayerRecord>(m => m.Loser)
                        .WithMany(t => t.LostGameResults)
                        .HasForeignKey(m => m.LoserId)
                        .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
