namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Runners;
    using Battleships.Web.Factories;
    using Battleships.Web.Helper;
    using System;
    using System.Collections.Generic;
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
            var latestLeagueTime = leagueRecordsRepository.GetLatestLeagueTime();

            ViewBag.LatestLeagueTime = latestLeagueTime;

            var matchResults = matchResultsRepository.GetAll().ToList();
            var leaderboard = leaderboardFactory.GenerateLeaderboard(matchResults);
            return View(leaderboard);
        }

        [HttpPost]
        public virtual ActionResult RunLeague()
        {
            var leagueStartTime = DateTime.Now;
            var players = GetPlayers();
            var updatedPlayers = players.Where(player => player.PlayerRecord.LastUpdated >= leagueRecordsRepository.GetLatestLeagueTime()).ToList();

            var matchResults = leagueRunner.GetLeagueResults(players, updatedPlayers);
            matchResultsRepository.UpdateResults(matchResults);
            matchResultsRepository.SaveContext();

            if (updatedPlayers.Count == 0)
            {
                TempData.AddPopup("Couldn't run league because no players have been updated since the last league. Please update or upload players and try again.", PopupType.Warning);
            }

            leagueRecordsRepository.AddLeague(leagueStartTime);
            leagueRecordsRepository.SaveContext();

            return RedirectToAction(MVC.League.Index());
        }

        private List<IBattleshipsPlayer> GetPlayers()
        {
            return playerRecordsRepository.GetAll().Select(p => battleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecord(p)).ToList();
        }
    }
}