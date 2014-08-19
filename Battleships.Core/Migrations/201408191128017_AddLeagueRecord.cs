namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddLeagueRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeagueRecords",
                c => new
                     {
                         Id = c.Int(false, true),
                         StartTime = c.DateTime(false),
                     })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.LeagueRecords");
        }
    }
}