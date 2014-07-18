namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public interface IPlayerRecordsRepository : IRepository<PlayerRecord>
    {
        PlayerRecord GetPlayerRecordById(int id);
        bool PlayerNameExists(string botName);
        bool PlayerNameExistsForUser(string botName, string userName);
    };

    [ExcludeFromCodeCoverage]
    public class PlayerRecordsRepository : Repository<PlayerRecord>, IPlayerRecordsRepository
    {
        public PlayerRecordsRepository(BattleshipsContext context) : base(context) {}

        public PlayerRecord GetPlayerRecordById(int id)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public bool PlayerNameExists(string botName)
        {
            var playerRecord = Entities.AsQueryable().FirstOrDefault(x => x.Name == botName);
            if (playerRecord != null)
            {
                return true;
            }
            return false;
        }

        public bool PlayerNameExistsForUser(string botName, string userName)
        {
            var playerRecord = Entities.AsQueryable().FirstOrDefault(x => x.Name == botName);
            if (playerRecord != null)
            {
                return playerRecord.UserName == userName;
            }
            return false;
        }
    }
}