namespace Battleships.Web.Services
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPlayerDeletionService
    {
        void DeleteRecordsByPlayerId(int id);
    }

    public class PlayerDeletionService : IPlayerDeletionService
    {
        private readonly PlayerRecordsRepository playerRecordsRepository;
        private readonly MatchResultsRepository matchResultsRepository;
        private readonly GameLogRepository gameLogRepository;

        public PlayerDeletionService(PlayerRecordsRepository playerRecordsRepository, MatchResultsRepository matchResultsRepository, GameLogRepository gameLogRepository)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.matchResultsRepository = matchResultsRepository;
            this.gameLogRepository = gameLogRepository;
        }

        public void DeleteRecordsByPlayerId(int id)
        {
            var playersToBeDeleted = new List<PlayerRecord> { playerRecordsRepository.GetPlayerRecordById(id) };
            DeleteMatchResultsByPlayerId(id);
            DeleteGameLogsByPlayerId(id);
            playerRecordsRepository.RemoveRange(playersToBeDeleted);
            playerRecordsRepository.SaveContext();
        }

        private void DeleteGameLogsByPlayerId(int id)
        {
            gameLogRepository.RemoveRange(gameLogRepository.GetAll().Where(gameLog => gameLog.Player1.Id == id || gameLog.Player2.Id == id));
            gameLogRepository.SaveContext();
        }

        private void DeleteMatchResultsByPlayerId(int id)
        {
            matchResultsRepository.RemoveRange(matchResultsRepository.GetAll().Where(matchResult => matchResult.WinnerId == id || matchResult.LoserId == id));
            matchResultsRepository.SaveContext();
        }
    }
}