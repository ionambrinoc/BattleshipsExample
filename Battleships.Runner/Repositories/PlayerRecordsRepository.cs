namespace Battleships.Runner.Repositories
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPlayerRecordsRepository : IRepository<PlayerRecord>
    {
        PlayerRecord GetPlayerRecordById(int id);
        bool PlayerNameExists(string botName);
        bool PlayerNameExistsForUser(string botName, string userName);
        PlayerRecord GetPlayerRecordFromBattleshipsPlayer(IBattleshipsPlayer battleshipsPlayer);
        IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId);
        IEnumerable<PlayerRecord> GetAllForUserName(string userName);
        void DeletePlayerRecordById(int id);
    };

    public class PlayerRecordsRepository : Repository<PlayerRecord>, IPlayerRecordsRepository
    {
        private readonly IPlayerLoader playerLoader;
        private readonly BattleshipsContext context;

        public PlayerRecordsRepository(BattleshipsContext context, IPlayerLoader playerLoader)
            : base(context)
        {
            this.context = context;
            this.playerLoader = playerLoader;
        }

        public PlayerRecord GetPlayerRecordById(int id)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public bool PlayerNameExists(string playerName)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Name == playerName) != null;
        }

        public bool PlayerNameExistsForUser(string playerName, string userName)
        {
            return Entities.AsQueryable().Any(x => x.Name == playerName && x.UserName == userName);
        }

        public PlayerRecord GetPlayerRecordFromBattleshipsPlayer(IBattleshipsPlayer battleshipsPlayer)
        {
            return Entities.FirstOrDefault(playerRecord => playerRecord.Name == battleshipsPlayer.Name);
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId)
        {
            var player = GetPlayerRecordById(playerRecordId);
            return playerLoader.GetBattleshipsPlayerFromPlayerName(player.Name);
        }

        public IEnumerable<PlayerRecord> GetAllForUserName(string userName)
        {
            return GetAll().Where(playerRecord => playerRecord.UserName == userName);
        }

        public void DeletePlayerRecordById(int id)
        {
            var playerRecord = GetPlayerRecordById(id);
            context.MatchResults.RemoveRange(playerRecord.WonMatchResults.Concat(playerRecord.LostMatchResults));
            context.SaveChanges();
            Entities.Remove(playerRecord);
            SaveContext();
        }
    }
}
