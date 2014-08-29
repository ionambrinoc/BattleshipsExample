namespace Battleships.Web.Services
{
    using Battleships.Core.Repositories;

    public interface IPlayerDeletionService
    {
        void DeleteRecordsByPlayerId(int id);
    }

    public class PlayerDeletionService : IPlayerDeletionService
    {
        private readonly PlayerRecordsRepository playerRecordsRepository;
        private readonly MatchResultsRepository matchResultsRepository;
        private readonly PlayerUploadService playerUploadService;

        public PlayerDeletionService(PlayerRecordsRepository playerRecordsRepository, MatchResultsRepository matchResultsRepository, PlayerUploadService playerUploadService)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.matchResultsRepository = matchResultsRepository;
            this.playerUploadService = playerUploadService;
        }

        public void DeleteRecordsByPlayerId(int id)
        {
            var playerToDelete = playerRecordsRepository.GetPlayerRecordById(id);

            playerUploadService.DeleteFilesForPlayer(playerToDelete.Name, playerToDelete.PictureFileName);

            matchResultsRepository.DeleteAllForPlayerId(id);
            playerRecordsRepository.Delete(playerToDelete);
            playerRecordsRepository.SaveContext();
        }
    }
}
