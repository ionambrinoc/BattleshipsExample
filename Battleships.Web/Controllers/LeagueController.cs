namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Models.League;
    using System.Linq;
    using System.Web.Mvc;

    public partial class LeagueController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly ILeagueRunner leagueRunner;
        private readonly ILeaderboardViewModel leaderboardViewModel;

        public LeagueController(IPlayerRecordsRepository playerRecordsRepository, ILeagueRunner leagueRunner,
                                ILeaderboardViewModel leaderboardViewModel)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.leagueRunner = leagueRunner;
            this.leaderboardViewModel = leaderboardViewModel;
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
            return Json(leaderboardViewModel.GenerateLeaderboard(leagueRunner.GetLeagueResults(battleshipsPlayers, 3)));
        }
    }
}