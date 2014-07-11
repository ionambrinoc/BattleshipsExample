namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;

    public interface IGameResultsRepository : IRepository<GameResult> {}

    public class GameResultsRepository : Repository<GameResult>, IGameResultsRepository
    {
        public GameResultsRepository(BattleshipsContext context) : base(context) {}
    }
}