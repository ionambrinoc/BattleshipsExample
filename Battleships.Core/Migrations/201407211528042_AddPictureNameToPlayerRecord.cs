namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPictureNameToPlayerRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerRecords", "PictureName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.PlayerRecords", "PictureName");
        }
    }
}
