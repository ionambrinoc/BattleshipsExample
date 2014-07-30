namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenamePictureNameToPictureFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerRecords", "PictureFileName", c => c.String());
            DropColumn("dbo.PlayerRecords", "PictureName");
        }

        public override void Down()
        {
            AddColumn("dbo.PlayerRecords", "PictureName", c => c.String());
            DropColumn("dbo.PlayerRecords", "PictureFileName");
        }
    }
}
