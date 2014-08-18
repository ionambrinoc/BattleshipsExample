namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System.Data.Entity;

    public interface IGameLogRepository
    {
        void AddGameLog(IGameLog gameLog);
    }

    public class GameLogRepository : Repository<IGameLog>, IGameLogRepository
    {
        public GameLogRepository(DbContext context) : base(context) {}

        public void AddGameLog(IGameLog gameLog)
        {
            Entities.Add(gameLog);
        }
    }
}