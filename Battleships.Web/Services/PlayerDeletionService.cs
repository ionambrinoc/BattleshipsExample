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

        public PlayerDeletionService(PlayerRecordsRepository playerRecordsRepository, MatchResultsRepository matchResultsRepository)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.matchResultsRepository = matchResultsRepository;
        }

        public void DeleteRecordsByPlayerId(int id)
        {
            var playersToBeDeleted = new List<PlayerRecord> { playerRecordsRepository.GetPlayerRecordById(id) };
            DeleteMatchResultsByPlayerId(id);
            playerRecordsRepository.RemoveRange(playersToBeDeleted);
            playerRecordsRepository.SaveContext();
        }

        private void DeleteMatchResultsByPlayerId(int id)
        {
            matchResultsRepository.RemoveRange(matchResultsRepository.GetAll().Where(matchResult => matchResult.WinnerId == id || matchResult.LoserId == id));
            matchResultsRepository.SaveContext();
        }
    }
}