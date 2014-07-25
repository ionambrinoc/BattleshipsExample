namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Factories;
    using Battleships.Web.Models.League;
    using System.Collections.Generic;
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

            var playerRecords = playerRecordsRepository.GetAll();
            var model = playerRecords.Select(playerRecord => new LeaguePlayerRecordViewModel
                                                             {
                                                                 PlayerName = playerRecord.Name,
                                                                 PlayerId = playerRecord.Id,
                                                                 PictureFileName = playerRecord.PictureFileName,
                                                                 UserName = playerRecord.User.UserName,
                                                                 Checked = false
                                                             }).ToList();
            return View(model);
        }

        [HttpPost]

        public virtual ActionResult RunLeague(IEnumerable<LeaguePlayerRecordViewModel> model)
        {

            var battleshipsPlayers = model.Where(m => m.Checked).Select(m => playerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(m.PlayerId)).ToList();
            var matchResults = leagueRunner.GetLeagueResults(battleshipsPlayers, 3);
            matchResultsRepository.AddResults(matchResults);
            var leaderboard = leaderboardFactory.GenerateLeaderboard(matchResults);
            return Json(leaderboard);
        }
    }
}
