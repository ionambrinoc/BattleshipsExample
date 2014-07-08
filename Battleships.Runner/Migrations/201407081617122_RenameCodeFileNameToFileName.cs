namespace Battleships.Runner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameCodeFileNameToFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "FileName", c => c.String());
            DropColumn("dbo.Players", "CodeFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "CodeFileName", c => c.String());
            DropColumn("dbo.Players", "FileName");
        }
    }
}
