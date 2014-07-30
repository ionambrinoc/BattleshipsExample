namespace Battleships.Core.Repositories
{
    using Battleships.Core.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public interface IPlayerRecordsRepository : IRepository<PlayerRecord>
    {
        PlayerRecord GetPlayerRecordById(int id);
        bool PlayerNameExists(string botName);
        bool PlayerNameExistsForUser(string botName, string userId);
        IEnumerable<PlayerRecord> GetAllForUserId(string userId);
        void DeletePlayerRecordById(int id);
    };

    public class PlayerRecordsRepository : Repository<PlayerRecord>, IPlayerRecordsRepository
    {
        private readonly DbContext context;

        public PlayerRecordsRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public PlayerRecord GetPlayerRecordById(int id)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public bool PlayerNameExists(string playerName)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Name == playerName) != null;
        }

        public bool PlayerNameExistsForUser(string playerName, string userId)
        {
            return Entities.AsQueryable().Any(x => x.Name == playerName && x.UserId == userId);
        }

        public IEnumerable<PlayerRecord> GetAllForUserId(string userId)
        {
            return GetAll().Where(playerRecord => playerRecord.UserId == userId);
        }

        public void DeletePlayerRecordById(int id)
        {
            var playerRecord = GetPlayerRecordById(id);
            context.Set<MatchResult>().RemoveRange(playerRecord.WonMatchResults.Concat(playerRecord.LostMatchResults));
            context.SaveChanges();
            Entities.Remove(playerRecord);
            SaveContext();
        }
    }
}
