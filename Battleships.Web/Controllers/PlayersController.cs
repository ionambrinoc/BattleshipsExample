namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IHeadToHeadRunner headToHeadRunner;
        private readonly IGameResultsRepository gameResultsRepository;

        public PlayersController(IPlayerRecordsRepository playerRecordsRepository, IGameResultsRepository gameResultsRepository,
                                 IHeadToHeadRunner headToHeadRunner)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.headToHeadRunner = headToHeadRunner;
            this.gameResultsRepository = gameResultsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAll().Reverse());
        }

        [HttpPost]
        public virtual ActionResult RunGame(int playerOneId, int playerTwoId)
        {
            var battleshipsPlayerOne = playerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(playerOneId);
            var battleshipsPlayerTwo = playerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(playerTwoId);
            var winner = headToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo);
            var loser = winner == battleshipsPlayerOne ? battleshipsPlayerTwo : battleshipsPlayerOne;
            var gameResult = new GameResult
                             {
                                 WinnerId = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(winner).Id,
                                 LoserId = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(loser).Id,
                                 TimePlayed = DateTime.Now
                             };
            gameResultsRepository.Add(gameResult);
            gameResultsRepository.SaveContext();
            return Json(winner.Name);
        }
    }
}
