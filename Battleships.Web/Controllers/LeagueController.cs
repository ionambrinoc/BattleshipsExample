namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Runners;
    using Battleships.Web.Factories;
    using System.Linq;
    using System.Web.Mvc;

    public partial class LeagueController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IBattleshipsPlayerRepository battleshipsPlayerRepository;
        private readonly ILeagueRunner leagueRunner;
        private readonly ILeaderboardFactory leaderboardFactory;
        private readonly IMatchResultsRepository matchResultsRepository;

        public LeagueController(IPlayerRecordsRepository playerRecordsRepository, IBattleshipsPlayerRepository battleshipsPlayerRepository, ILeagueRunner leagueRunner,
                                ILeaderboardFactory leaderboardFactory, IMatchResultsRepository matchResultsRepository)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.battleshipsPlayerRepository = battleshipsPlayerRepository;
            this.leagueRunner = leagueRunner;
            this.leaderboardFactory = leaderboardFactory;
            this.matchResultsRepository = matchResultsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult RunLeague()
        {
            var players = playerRecordsRepository.GetAll().Select(p => battleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecord(p)).ToList();
            var updatedPlayers = players.Where(player => player.PlayerRecord.LastUpdated >= player.PlayerRecord.GetLastTimePlayed()).ToList();

            var matchResults = leagueRunner.GetLeagueResults(players, updatedPlayers);

            matchResultsRepository.UpdateResults(matchResults);
            matchResultsRepository.SaveContext();

            var leaderboard = leaderboardFactory.GenerateLeaderboard(matchResults);
            return Json(leaderboard);
        }
    }
}