namespace Battleships.Web.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Services;
    using Battleships.Web.Models.AddPlayer;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IBattleshipsPlayerRepository battleshipsPlayerRepository;
        private readonly IHeadToHeadRunner headToHeadRunner;
        private readonly IMatchResultsRepository matchResultsRepository;

        public PlayersController(IPlayerRecordsRepository playerRecordsRepository, IBattleshipsPlayerRepository battleshipsPlayerRepository, IMatchResultsRepository matchResultsRepository,
                                 IHeadToHeadRunner headToHeadRunner)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.battleshipsPlayerRepository = battleshipsPlayerRepository;
            this.headToHeadRunner = headToHeadRunner;
            this.matchResultsRepository = matchResultsRepository;
            this.matchResultsRepository = matchResultsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAll().Reverse().Select(m => new PlayerRecordViewModel(m)));
        }

        [HttpPost]
        public virtual JsonResult RunGame(int playerOneId, int playerTwoId)
        {
            var battleshipsPlayerOne = battleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecordId(playerOneId);
            var battleshipsPlayerTwo = battleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecordId(playerTwoId);
            var winnerResult = headToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo);
            var winner = winnerResult.Winner;
            var loser = winner == battleshipsPlayerOne ? battleshipsPlayerTwo : battleshipsPlayerOne;
            var matchResult = new MatchResult
                              {
                                  Winner = winner.PlayerRecord,
                                  Loser = loser.PlayerRecord,
                                  WinnerWins = 1,
                                  LoserWins = 0,
                                  TimePlayed = DateTime.Now
                              };
            matchResultsRepository.Add(matchResult);
            matchResultsRepository.SaveContext();
            return Json(new { winnerName = winner.Name, resultType = (int)winnerResult.ResultType });
        }
    }
}
