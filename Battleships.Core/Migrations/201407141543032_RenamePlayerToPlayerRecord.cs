namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenamePlayerToPlayerRecord : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.Players", "PlayerRecords");
        }

        public override void Down()
        {
            RenameTable("dbo.PlayerRecords", "Players");
        }
    }
}
