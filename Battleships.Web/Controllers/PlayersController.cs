namespace Battleships.Web.Controllers
{
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Runners;
    using System.Linq;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IBattleshipsPlayerRepository battleshipsPlayerRepository;
        private readonly IHeadToHeadRunner headToHeadRunner;

        public PlayersController(IPlayerRecordsRepository playerRecordsRepository, IBattleshipsPlayerRepository battleshipsPlayerRepository, IHeadToHeadRunner headToHeadRunner)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.battleshipsPlayerRepository = battleshipsPlayerRepository;
            this.headToHeadRunner = headToHeadRunner;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAll().Reverse());
        }

        [HttpPost]
        public virtual JsonResult RunGame(int playerOneId, int playerTwoId)
        {
            var battleshipsPlayerOne = battleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecordId(playerOneId);
            var battleshipsPlayerTwo = battleshipsPlayerRepository.GetBattleshipsPlayerFromPlayerRecordId(playerTwoId);
            var logger = new Logger(battleshipsPlayerOne, battleshipsPlayerTwo);
            var result = headToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo, logger);
            return Json(new { winnerName = result.Winner.Name, resultType = (int)result.ResultType });
        }
    }
}