namespace Battleships.Runner
{
    using Battleships.Runner.Migrations;
    using System.Data.Entity;

    public class BattleshipsInitializer : MigrateDatabaseToLatestVersion<BattleshipsContext, Configuration> {}
}
