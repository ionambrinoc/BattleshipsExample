namespace Battleships.Core
{
    using Battleships.Runner.Migrations;
    using System.Data.Entity;

    public class BattleshipsInitializer : MigrateDatabaseToLatestVersion<BattleshipsContext, Configuration> {}
}
