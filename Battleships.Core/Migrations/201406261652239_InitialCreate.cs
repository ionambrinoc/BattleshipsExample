namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kittens",
                c => new
                     {
                         Id = c.Int(false, true),
                         Name = c.String(),
                     })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Kittens");
        }
    }
}
