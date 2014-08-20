namespace Battleships.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGameLogAndGameEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        Player1Won = c.Boolean(nullable: false),
                        ResultType = c.Int(nullable: false),
                        Player1PositionsString = c.String(),
                        Player2PositionsString = c.String(),
                        Player1_Id = c.Int(),
                        Player2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlayerRecords", t => t.Player1_Id)
                .ForeignKey("dbo.PlayerRecords", t => t.Player2_Id)
                .Index(t => t.Player1_Id)
                .Index(t => t.Player2_Id);
            
            CreateTable(
                "dbo.GameEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        IsPlayer1Turn = c.Boolean(nullable: false),
                        SelectedTarget = c.String(),
                        IsHit = c.Boolean(nullable: false),
                        GameLogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameLogs", t => t.GameLogId, cascadeDelete: true)
                .Index(t => t.GameLogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameLogs", "Player2_Id", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameLogs", "Player1_Id", "dbo.PlayerRecords");
            DropForeignKey("dbo.GameEvents", "GameLogId", "dbo.GameLogs");
            DropIndex("dbo.GameEvents", new[] { "GameLogId" });
            DropIndex("dbo.GameLogs", new[] { "Player2_Id" });
            DropIndex("dbo.GameLogs", new[] { "Player1_Id" });
            DropTable("dbo.GameEvents");
            DropTable("dbo.GameLogs");
        }
    }
}
