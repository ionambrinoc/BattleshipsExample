namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenameGameResultToMatchResult : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameResults", "Loser_Id", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameResults", "Winner_Id", "dbo.PlayerRecords");
            DropIndex("dbo.GameResults", new[] { "Loser_Id" });
            DropIndex("dbo.GameResults", new[] { "Winner_Id" });
            CreateTable(
                "dbo.MatchResults",
                c => new
                     {
                         Id = c.Int(false, true),
                         WinnerId = c.Int(false),
                         LoserId = c.Int(false),
                         WinnerWins = c.Int(false),
                         LoserWins = c.Int(false),
                         TimePlayed = c.DateTime(false),
                     })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlayerRecords", t => t.LoserId)
                .ForeignKey("dbo.PlayerRecords", t => t.WinnerId)
                .Index(t => t.WinnerId)
                .Index(t => t.LoserId);

            DropTable("dbo.GameResults");
        }

        public override void Down()
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
                .PrimaryKey(t => t.Id);

            DropForeignKey("dbo.MatchResults", "WinnerId", "dbo.PlayerRecords");
            DropForeignKey("dbo.MatchResults", "LoserId", "dbo.PlayerRecords");
            DropIndex("dbo.MatchResults", new[] { "LoserId" });
            DropIndex("dbo.MatchResults", new[] { "WinnerId" });
            DropTable("dbo.MatchResults");
            CreateIndex("dbo.GameResults", "Winner_Id");
            CreateIndex("dbo.GameResults", "Loser_Id");
            AddForeignKey("dbo.GameResults", "Winner_Id", "dbo.PlayerRecords", "Id");
            AddForeignKey("dbo.GameResults", "Loser_Id", "dbo.PlayerRecords", "Id");
        }
    }
}
