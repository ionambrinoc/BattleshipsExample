namespace Battleships.Runner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameBotNameToName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Name", c => c.String());
            DropColumn("dbo.Players", "BotName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "BotName", c => c.String());
            DropColumn("dbo.Players", "Name");
        }
    }
}
