namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public interface IPlayerRecordsRepository : IRepository<PlayerRecord>
    {
        PlayerRecord GetPlayerRecordById(int id);
        bool GivenFileNameExists(string fileName);
        bool UpdatePlayerRecord(string fileName, string botName);
        PlayerRecord GivenBotNameExists(string botName);
    };

    [ExcludeFromCodeCoverage]
    public class PlayerRecordsRepository : Repository<PlayerRecord>, IPlayerRecordsRepository
    {
        public PlayerRecordsRepository(BattleshipsContext context) : base(context) {}

        public PlayerRecord GetPlayerRecordById(int id)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public bool GivenFileNameExists(string fileName)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.FileName == fileName) != null;
        }

        public PlayerRecord GivenBotNameExists(string botName)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Name == botName);
        }

        public bool UpdatePlayerRecord(string fileName, string botName)
        {
            var playerRecord = Entities.AsQueryable().First(x => x.FileName == fileName);
            if (playerRecord != null)
            {
                Entities.Remove(playerRecord);
                SaveContext();
                playerRecord.Name = botName;
                Add(playerRecord);
                SaveContext();
            }
            return true;
        }
    }
}