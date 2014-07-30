namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveKittens : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Kittens");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Kittens",
                c => new
                     {
                         Id = c.Int(false, true),
                         Name = c.String(),
                         FileName = c.String(),
                     })
                .PrimaryKey(t => t.Id);
        }
    }
}
