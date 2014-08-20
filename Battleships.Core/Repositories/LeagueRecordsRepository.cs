namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public interface ILeagueRecordsRepository : IRepository<LeagueRecord>
    {
        void AddLeague(DateTime startTime);
        DateTime GetLatestLeagueTime();
        int GetCount();
    }

    public class LeagueRecordsRepository : Repository<LeagueRecord>, ILeagueRecordsRepository
    {
        public LeagueRecordsRepository(DbContext context) : base(context) {}

        public void AddLeague(DateTime startTime)
        {
            Entities.Add(new LeagueRecord { StartTime = startTime });
        }

        public DateTime GetLatestLeagueTime()
        {
            return !Entities.Any() ? DateTime.MinValue : Entities.Max(league => league.StartTime);
        }

        public int GetCount()
        {
            return Entities.Count();
        }
    }
}