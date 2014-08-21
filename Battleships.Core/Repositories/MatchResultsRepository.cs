namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public interface IMatchResultsRepository : IRepository<MatchResult>
    {

        void UpdateResults(IEnumerable<MatchResult> results);
        IEnumerable<MatchResult> GetAllMatchResults(IEnumerable<int> playerIds);
    }


    public class MatchResultsRepository : Repository<MatchResult>, IMatchResultsRepository
    {
        public MatchResultsRepository(DbContext context) : base(context) {}

        public void UpdateResults(IEnumerable<MatchResult> results)
        {
            var newResults = results.Where(result => !TryUpdateResult(result)).ToList();
            AddResults(newResults);
        }

        public IEnumerable<MatchResult> GetAllMatchResults(IEnumerable<int> playerIds)
        {

            var query = Entities.Where(x => x.Id == 1).ToList();
        }
        private bool TryUpdateResult(MatchResult newResult)
        {
            foreach (var existingResult in Entities.AsEnumerable().Where(entity => entity.SamePlayers(newResult)))
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