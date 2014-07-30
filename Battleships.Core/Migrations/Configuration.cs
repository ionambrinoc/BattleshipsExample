namespace Battleships.Runner.Migrations
{
    using Battleships.Core;
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<BattleshipsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BattleshipsContext context) {}
    }
}
