namespace Battleships.Runner.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedForeignKeyToUsersTableInPlayerRecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerRecords", "UserId", c => c.String(true, 128));

            Sql(@"
UPDATE dbo.PlayerRecords
SET UserId = U.Id
FROM dbo.PlayerRecords P
JOIN dbo.AspNetUsers U
ON P.UserName = U.UserName");

            AlterColumn("dbo.PlayerRecords", "UserId", c => c.String(false, 128));

            CreateIndex("dbo.PlayerRecords", "UserId");
            AddForeignKey("dbo.PlayerRecords", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.PlayerRecords", "UserName");
        }

        public override void Down()
        {
            AddColumn("dbo.PlayerRecords", "UserName", c => c.String());

            Sql(@"
UPDATE dbo.PlayerRecords
SET UserName = U.UserName
FROM dbo.PlayerRecords P
JOIN dbo.AspNetUsers U
ON P.UserId = U.Id");

            DropForeignKey("dbo.PlayerRecords", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PlayerRecords", new[] { "UserId" });
            DropColumn("dbo.PlayerRecords", "UserId");
        }
    }
}
