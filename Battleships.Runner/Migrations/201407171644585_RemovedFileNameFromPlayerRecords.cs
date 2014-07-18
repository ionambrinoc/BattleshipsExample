namespace Battleships.Runner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedFileNameFromPlayerRecords : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PlayerRecords", "FileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayerRecords", "FileName", c => c.String());
        }
    }
}
