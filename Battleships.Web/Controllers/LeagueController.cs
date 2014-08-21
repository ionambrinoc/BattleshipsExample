namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Runners;
    using Battleships.Web.Factories;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public partial class LeagueController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IBattleshipsPlayerRepository battleshipsPlayerRepository;
        private readonly ILeagueRunner leagueRunner;
        private readonly ILeaderboardFactory leaderboardFactory;
        private readonly IMatchResultsRepository matchResultsRepository;
        private readonly ILeagueRecordsRepository leagueRecordsRepository;

        public LeagueController(IPlayerRecordsRepository playerRecordsRepository, IBattleshipsPlayerRepository battleshipsPlayerRepository, ILeagueRunner leagueRunner,
                                ILeaderboardFactory leaderboardFactory, IMatchResultsRepository matchResultsRepository, ILeagueRecordsRepository leagueRecordsRepository)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.battleshipsPlayerRepository = battleshipsPlayerRepository;
            this.leagueRunner = leagueRunner;
            this.leaderboardFactory = leaderboardFactory;
            this.matchResultsRepository = matchResultsRepository;
            this.leagueRecordsRepository = leagueRecordsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            var leagueCount = leagueRecordsRepository.GetCount();
            ViewBag.LeagueCount = leagueCount;
            ViewBag.LatestLeague = leagueRecordsRepository.GetLatestLeagueTime();
            return View();
        }

        [HttpGet]
        public virtual ActionResult LatestLeagureResults()
        {
            var latestLeagueTime = leagueRecordsRepository.GetLatestLeagueTime();
            var matchResults = matchResultsRepository.GetAll().Where(mr => mr.TimePlayed >= latestLeagueTime).ToList();
            var leaderboard = leaderboardFactory.GenerateLeaderboard(matchResults);

            return Json(leaderboard, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult RunLeague()
        {
            var leagueStartTime = DateTime.Now;
            var players = playerRecordsRepository.GetAll().Select(p => battleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecord(p)).ToList();
            var updatedPlayers = players.Where(player => player.PlayerRecord.LastUpdated >= leagueRecordsRepository.GetLatestLeagueTime()).ToList();

            var matchResults = leagueRunner.GetLeagueResults(players, updatedPlayers);

            matchResultsRepository.UpdateResults(matchResults);
            matchResultsRepository.SaveContext();

            var leaderboard = leaderboardFactory.GenerateLeaderboard(matchResults);

            if (updatedPlayers.Count > 0)
            {
                leagueRecordsRepository.AddLeague(leagueStartTime);
                leagueRecordsRepository.SaveContext();
            }

            return Json(leaderboard);
        }
    }
}