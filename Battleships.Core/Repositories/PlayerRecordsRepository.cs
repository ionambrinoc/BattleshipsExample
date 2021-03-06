﻿namespace Battleships.Core.Repositories
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

        void MarkPlayerAsUpdated(string playerName);
    };

    public class PlayerRecordsRepository : Repository<PlayerRecord>, IPlayerRecordsRepository
    {
        public PlayerRecordsRepository(DbContext context)
            : base(context) {}

        public PlayerRecord GetPlayerRecordById(int id)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public void MarkPlayerAsUpdated(string playerName)
        {
            GetByPlayerName(playerName).MarkAsUpdated();
        }

        public bool PlayerNameExists(string playerName)
        {
            return GetByPlayerName(playerName) != null;
        }

        public bool PlayerNameExistsForUser(string playerName, string userId)
        {
            return Entities.AsQueryable().Any(x => x.Name == playerName && x.UserId == userId);
        }

        public IEnumerable<PlayerRecord> GetAllForUserId(string userId)
        {
            return GetAll().Where(playerRecord => playerRecord.UserId == userId);
        }

        public void Delete(PlayerRecord playerToDelete)
        {
            Entities.Remove(playerToDelete);
        }

        private PlayerRecord GetByPlayerName(string playerName)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Name == playerName);
        }
    }
}
