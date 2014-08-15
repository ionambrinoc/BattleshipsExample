namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public interface IMatchResultsRepository : IRepository<MatchResult>
    {
        void UpdateResults(List<MatchResult> results);
        DateTime GetMostRecentMatchTime();
    }

    public class MatchResultsRepository : Repository<MatchResult>, IMatchResultsRepository
    {
        public MatchResultsRepository(DbContext context) : base(context) {}

        public void UpdateResults(List<MatchResult> results)
        {
            var newResults = results.Where(result => !TryUpdateResult(result)).ToList();
            AddResults(newResults);
        }

        public DateTime GetMostRecentMatchTime()
        {
            if (!GetAll().Any())
            {
                return DateTime.MinValue;
            }
            return GetAll().Max(x => x.TimePlayed);
        }

        private bool TryUpdateResult(MatchResult newResult)
        {
            foreach (var existingResult in Entities.ToList().Where(entity => entity.SamePlayers(newResult)))
            {
                existingResult.CopyFrom(newResult);
                return true;
            }
            return false;
        }

        private void AddResults(IEnumerable<MatchResult> newResults)
        {
            Entities.AddRange(newResults);
        }
    }
}