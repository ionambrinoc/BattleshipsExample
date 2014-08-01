namespace Battleships.Core
{
    using Battleships.Core.Migrations;
    using System.Data.Entity;

    public class BattleshipsInitializer : MigrateDatabaseToLatestVersion<BattleshipsContext, Configuration> {}
}
