namespace Battleships.Player
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;

    public interface IBattleshipsPlayerRepository
    {
        IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecord(PlayerRecord playerRecord);
        IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId);
    }

    public class BattleshipsPlayerRepository : IBattleshipsPlayerRepository
    {
        private readonly IPlayerRecordsRepository playerRecordsRepo;
        private readonly IBotLoader botLoader;

        public BattleshipsPlayerRepository(IPlayerRecordsRepository playerRecordsRepo, IBotLoader botLoader)
        {
            this.playerRecordsRepo = playerRecordsRepo;
            this.botLoader = botLoader;
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecord(PlayerRecord playerRecord)
        {
            return new BattleshipsPlayer(botLoader.LoadBotByName(playerRecord.Name), playerRecord);
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId)
        {
            return GetBattleshipsPlayerFromPlayerRecord(playerRecordsRepo.GetPlayerRecordById(playerRecordId));
        }
    }
}
