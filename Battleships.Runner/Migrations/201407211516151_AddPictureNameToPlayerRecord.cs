namespace Battleships.Runner.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPictureNameToPlayerRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerRecords", "PictureFileName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.PlayerRecords", "PictureFileName");
        }
    }
}