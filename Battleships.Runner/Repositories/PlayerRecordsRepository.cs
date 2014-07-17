namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public interface IPlayerRecordsRepository : IRepository<PlayerRecord>
    {
        PlayerRecord GetPlayerRecordById(int id);
        bool GivenFileNameExists(string fileName);
        string UserWithGivenBotName(string botName);
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

        public string UserWithGivenBotName(string botName)
        {
            var playerRecord = Entities.AsQueryable().FirstOrDefault(x => x.Name == botName);
            if (playerRecord != null)
            {
                return playerRecord.UserName;
            }
            return "";
        }
    }
}