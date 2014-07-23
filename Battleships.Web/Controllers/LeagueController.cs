namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Factories;
    using System.Linq;
    using System.Web.Mvc;

    public partial class LeagueController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly ILeagueRunner leagueRunner;
        private readonly ILeaderboardFactory leaderboardFactory;
        private readonly IMatchResultsRepository matchResultsRepository;

        public LeagueController(IPlayerRecordsRepository playerRecordsRepository, ILeagueRunner leagueRunner,
                                ILeaderboardFactory leaderboardFactory, IMatchResultsRepository matchResultsRepository)
        {
            this.playerRecordsRepository = playerRecordsRepository;
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
            var battleshipsPlayers = playerRecordsRepository.GetAll().Select(p => playerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(p.Id)).ToList();
            var matchResults = leagueRunner.GetLeagueResults(battleshipsPlayers, 3);
            matchResultsRepository.AddResults(matchResults);
            var leaderboard = leaderboardFactory.GenerateLeaderboard(matchResults);
            return Json(leaderboard);
        }
    }
}