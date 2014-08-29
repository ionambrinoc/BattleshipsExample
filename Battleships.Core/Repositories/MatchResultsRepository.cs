namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System;
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
            var playerIdsSet = new HashSet<int>(playerIds);
            return playerIdsSet.SelectMany(x => playerIdsSet.Where(y => y > x), FindResultBetween);
        }

        public override void Add(MatchResult entity)
        {
            throw new InvalidOperationException("The 'Add' method for MatchResultsRepository can lead to duplicate entries and should not be used. Use 'UpdateResults' instead.");
        }

        private MatchResult FindResultBetween(int firstPlayerId, int secondPlayerId)
        {
            return Entities.Single(result => (result.WinnerId == firstPlayerId && result.LoserId == secondPlayerId) || (result.WinnerId == secondPlayerId && result.LoserId == firstPlayerId));
        }

        private bool TryUpdateResult(MatchResult newResult)
        {
            foreach (var existingResult in Entities.AsEnumerable().Where(entity => entity.HasSamePlayersAs(newResult)))
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