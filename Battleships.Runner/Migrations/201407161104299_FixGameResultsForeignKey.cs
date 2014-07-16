namespace Battleships.Runner.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FixGameResultsForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameResults", "Loser_Id", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameResults", "Winner_Id", "dbo.PlayerRecords");
            DropIndex("dbo.GameResults", new[] { "Loser_Id" });
            DropIndex("dbo.GameResults", new[] { "Winner_Id" });
            RenameColumn("dbo.GameResults", "Loser_Id", "LoserId");
            RenameColumn("dbo.GameResults", "Winner_Id", "WinnerId");
            AddColumn("dbo.GameResults", "PlayerRecord_Id", c => c.Int());
            AddColumn("dbo.GameResults", "PlayerRecord_Id1", c => c.Int());
            AlterColumn("dbo.GameResults", "LoserId", c => c.Int(false));
            AlterColumn("dbo.GameResults", "WinnerId", c => c.Int(false));
            CreateIndex("dbo.GameResults", "WinnerId");
            CreateIndex("dbo.GameResults", "LoserId");
            CreateIndex("dbo.GameResults", "PlayerRecord_Id");
            CreateIndex("dbo.GameResults", "PlayerRecord_Id1");
            AddForeignKey("dbo.GameResults", "PlayerRecord_Id", "dbo.PlayerRecords", "Id");
            AddForeignKey("dbo.GameResults", "PlayerRecord_Id1", "dbo.PlayerRecords", "Id");
            AddForeignKey("dbo.GameResults", "LoserId", "dbo.PlayerRecords", "Id");
            AddForeignKey("dbo.GameResults", "WinnerId", "dbo.PlayerRecords", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GameResults", "WinnerId", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameResults", "LoserId", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameResults", "PlayerRecord_Id1", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameResults", "PlayerRecord_Id", "dbo.PlayerRecords");
            DropIndex("dbo.GameResults", new[] { "PlayerRecord_Id1" });
            DropIndex("dbo.GameResults", new[] { "PlayerRecord_Id" });
            DropIndex("dbo.GameResults", new[] { "LoserId" });
            DropIndex("dbo.GameResults", new[] { "WinnerId" });
            AlterColumn("dbo.GameResults", "WinnerId", c => c.Int());
            AlterColumn("dbo.GameResults", "LoserId", c => c.Int());
            DropColumn("dbo.GameResults", "PlayerRecord_Id1");
            DropColumn("dbo.GameResults", "PlayerRecord_Id");
            RenameColumn("dbo.GameResults", "WinnerId", "Winner_Id");
            RenameColumn("dbo.GameResults", "LoserId", "Loser_Id");
            CreateIndex("dbo.GameResults", "Winner_Id");
            CreateIndex("dbo.GameResults", "Loser_Id");
            AddForeignKey("dbo.GameResults", "Winner_Id", "dbo.PlayerRecords", "Id");
            AddForeignKey("dbo.GameResults", "Loser_Id", "dbo.PlayerRecords", "Id");
        }
    }
}
