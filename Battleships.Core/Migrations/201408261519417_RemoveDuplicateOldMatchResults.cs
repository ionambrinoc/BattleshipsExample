namespace Battleships.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveDuplicateOldMatchResults : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE MatchResults FROM MatchResults
                  LEFT OUTER JOIN
                      (SELECT M1.Id, MAX(M2.TimePlayed) AS MaxTimePlayed
                      FROM MatchResults M1
                      JOIN MatchResults M2
                      ON ((M1.WinnerId = M2.WinnerId AND M1.LoserId = M2.LoserId) OR (M1.WinnerId = M2.LoserId AND M1.LoserId = M2.WinnerId))
                      GROUP BY M1.Id) AS RowsToKeep
                  ON RowsToKeep.Id = MatchResults.Id
                  WHERE MatchResults.TimePlayed < MaxTimePlayed");
        }

        public override void Down() {}
    }
}