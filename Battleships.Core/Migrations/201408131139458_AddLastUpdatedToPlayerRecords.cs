namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddLastUpdatedToPlayerRecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerRecords", "LastUpdated", c => c.DateTime(false));
        }

        public override void Down()
        {
            DropColumn("dbo.PlayerRecords", "LastUpdated");
        }
    }
}