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
        private readonly IMatchResultsRepository matchResultsRepository;
        private readonly ILeagueRunner leagueRunner;
        private readonly ILeagueResults leagueResults;

        public PlayersController(IPlayerRecordsRepository playerRecordsRepository, IMatchResultsRepository matchResultsRepository,
                                 IHeadToHeadRunner headToHeadRunner, ILeagueRunner leagueRunner, ILeagueResults leagueResults)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.headToHeadRunner = headToHeadRunner;
            this.matchResultsRepository = matchResultsRepository;
            this.leagueRunner = leagueRunner;
            this.leagueResults = leagueResults;
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
            var matchResult = new MatchResult
                              {
                                  WinnerId = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(winner).Id,
                                  LoserId = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(loser).Id,
                                  WinnerWins = 1,
                                  LoserWins = 0,
                                  TimePlayed = DateTime.Now
                              };
            matchResultsRepository.Add(matchResult);
            matchResultsRepository.SaveContext();
            return Json(winner.Name);
        }

        [HttpPost]
        public virtual ActionResult RunLeague()
        {
            // Linq ftw
            var battleshipsPlayers = playerRecordsRepository.GetAll().Select(p => playerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(p.Id)).ToList();
            var orderedPlayers = leagueResults.GenerateLeaderboard(leagueRunner.GetLeagueResults(battleshipsPlayers, 3));
            return Json(orderedPlayers);
        }
    }
}