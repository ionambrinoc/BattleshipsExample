namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddFileNameToKitten : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kittens", "FileName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Kittens", "FileName");
        }
    }
}
