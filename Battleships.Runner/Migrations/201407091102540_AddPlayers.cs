namespace Battleships.Runner.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Players : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                     {
                         Id = c.Int(false, true),
                         UserName = c.String(),
                         Name = c.String(),
                         FileName = c.String(),
                     })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Players");
        }
    }
}