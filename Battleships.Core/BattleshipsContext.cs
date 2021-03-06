﻿namespace Battleships.Core
{
    using Battleships.Core.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class BattleshipsContext : IdentityDbContext<User>
    {
        public BattleshipsContext() : base("DefaultConnection") {}

        public BattleshipsContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<PlayerRecord> PlayerRecords { get; set; }

        public DbSet<MatchResult> MatchResults { get; set; }

        public DbSet<LeagueRecord> LeagueRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerRecord>()
                        .HasRequired<User>(m => m.User)
                        .WithMany(m => m.PlayerRecords)
                        .HasForeignKey(m => m.UserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<MatchResult>()
                        .HasRequired<PlayerRecord>(m => m.Winner)
                        .WithMany(t => t.WonMatchResults)
                        .HasForeignKey(m => m.WinnerId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<MatchResult>()
                        .HasRequired<PlayerRecord>(m => m.Loser)
                        .WithMany(t => t.LostMatchResults)
                        .HasForeignKey(m => m.LoserId)
                        .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}