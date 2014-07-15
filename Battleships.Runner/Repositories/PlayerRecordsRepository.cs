namespace Battleships.Runner.Repositories
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public interface IPlayerRecordsRepository : IRepository<PlayerRecord>
    {
        PlayerRecord GetPlayerRecordById(int id);
        IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId);
        PlayerRecord GetPlayerRecordFromBattleshipsPlayer(IBattleshipsPlayer battleshipsPlayer);
    };

    [ExcludeFromCodeCoverage]
    public class PlayerRecordsRepository : Repository<PlayerRecord>, IPlayerRecordsRepository
    {
        private readonly IPlayerLoader playerLoader;

        public PlayerRecordsRepository(BattleshipsContext context, IPlayerLoader playerLoader) : base(context)
        {
            this.playerLoader = playerLoader;
        }

        public PlayerRecord GetPlayerRecordById(int id)
        {
            return Entities.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId)
        {
            var player = GetPlayerRecordById(playerRecordId);
            return playerLoader.GetPlayerFromFile(player.FileName);
        }

        public PlayerRecord GetPlayerRecordFromBattleshipsPlayer(IBattleshipsPlayer battleshipsPlayer)
        {
            return Entities.FirstOrDefault(playerRecord => playerRecord.Name == battleshipsPlayer.Name);
        }
    }
}