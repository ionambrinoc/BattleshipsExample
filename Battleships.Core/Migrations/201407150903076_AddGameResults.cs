namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddGameResults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameResults",
                c => new
                     {
                         Id = c.Int(false, true),
                         TimePlayed = c.DateTime(false),
                         Loser_Id = c.Int(),
                         Winner_Id = c.Int(),
                     })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlayerRecords", t => t.Loser_Id)
                .ForeignKey("dbo.PlayerRecords", t => t.Winner_Id)
                .Index(t => t.Loser_Id)
                .Index(t => t.Winner_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.GameResults", "Winner_Id", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameResults", "Loser_Id", "dbo.PlayerRecords");
            DropIndex("dbo.GameResults", new[] { "Winner_Id" });
            DropIndex("dbo.GameResults", new[] { "Loser_Id" });
            DropTable("dbo.GameResults");
        }
    }
}
