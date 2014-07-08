﻿namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class BattleshipsContext : IdentityDbContext<User>
    {
        public BattleshipsContext() : base("DefaultConnection") {}

        public BattleshipsContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<Kitten> Kittens { get; set; }
    }
}
