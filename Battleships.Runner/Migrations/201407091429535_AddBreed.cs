namespace Battleships.Runner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBreed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kittens", "Breed", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kittens", "Breed");
        }
    }
}
