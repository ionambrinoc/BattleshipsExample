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
        private readonly IPlayerLoader playerLoader;

        public BattleshipsPlayerRepository(IPlayerRecordsRepository playerRecordsRepo, IPlayerLoader playerLoader)
        {
            this.playerRecordsRepo = playerRecordsRepo;
            this.playerLoader = playerLoader;
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecord(PlayerRecord playerRecord)
        {
            return new BattleshipsPlayer(playerLoader.LoadBotByName(playerRecord.Name), playerRecord);
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerRecordId(int playerRecordId)
        {
            return GetBattleshipsPlayerFromPlayerRecord(playerRecordsRepo.GetPlayerRecordById(playerRecordId));
        }
    }
}
