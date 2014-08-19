namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System.Data.Entity;

    public interface IGameLogRepository : IRepository<GameLog>
    {
        void AddGameLog(GameLog gameLog);
    }

    public class GameLogRepository : Repository<GameLog>, IGameLogRepository
    {
        public GameLogRepository(DbContext context) : base(context) {}

        public void AddGameLog(GameLog gameLog)
        {
            Entities.Add(gameLog);
        }
    }
}