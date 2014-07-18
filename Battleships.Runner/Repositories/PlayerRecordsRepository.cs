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

        public bool PlayerNameExists(string playerName)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Name == playerName) != null;
        }

        public bool PlayerNameExistsForUser(string playerName, string userName)
        {
            return Entities.AsQueryable().Any(x => x.Name == playerName && x.UserName == userName);
        }
    }
}