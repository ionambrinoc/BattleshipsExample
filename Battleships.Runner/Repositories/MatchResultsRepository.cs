namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;
    using System.Collections.Generic;

    public interface IMatchResultsRepository : IRepository<MatchResult>
    {
        void AddResults(List<MatchResult> results);
    }

    public class MatchResultsRepository : Repository<MatchResult>, IMatchResultsRepository
    {
        public MatchResultsRepository(BattleshipsContext context)
            : base(context) {}

        public void AddResults(List<MatchResult> results)
        {
            Entities.AddRange(results);
            SaveContext();
        }
    }
}