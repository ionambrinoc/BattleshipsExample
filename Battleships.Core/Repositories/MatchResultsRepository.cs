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
        List<MatchResult> GetAllMatchResults(IEnumerable<int> playerIds);
    }

    public class MatchResultsRepository : Repository<MatchResult>, IMatchResultsRepository
    {
        public MatchResultsRepository(DbContext context) : base(context) {}

        public void UpdateResults(IEnumerable<MatchResult> results)
        {
            var newResults = results.Where(result => !TryUpdateResult(result)).ToList();
            AddResults(newResults);
        }

        public List<MatchResult> GetAllMatchResults(IEnumerable<int> playerIds)
        {
            var playerIdsSet = new HashSet<int>(playerIds);
            var playerIdsSetCopy = new HashSet<int>(playerIds);
            var allMatchResults = new List<MatchResult>();
            foreach (var firstPlayerId in playerIdsSet)
            {
                playerIdsSetCopy.Remove(firstPlayerId);
                allMatchResults.AddRange(playerIdsSetCopy.Select(secondPlayerId => FindResultBetween(firstPlayerId, secondPlayerId)));
            }
            return allMatchResults;
        }

        private MatchResult FindResultBetween(int firstPlayerId, int secondPlayerId)
        {
            foreach (var matchResult in Entities.Where(result => (result.WinnerId == firstPlayerId && result.LoserId == secondPlayerId) || (result.WinnerId == secondPlayerId && result.LoserId == firstPlayerId)))
            {
                return matchResult;
            }
            throw new Exception();
            return null;
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