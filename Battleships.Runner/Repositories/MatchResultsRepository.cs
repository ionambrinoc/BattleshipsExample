namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;

    public interface IMatchResultsRepository : IRepository<MatchResult> {}

    public class MatchResultsRepository : Repository<MatchResult>, IMatchResultsRepository
    {
        public MatchResultsRepository(BattleshipsContext context)
            : base(context) {}
    }
}