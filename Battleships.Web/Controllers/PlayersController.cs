namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Repositories;
    using System.Linq;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IPlayerLoader playerLoader;
        private readonly IHeadToHeadRunner headToHeadRunner;

        public PlayersController(IPlayerRecordsRepository playerRecordsRepository, IPlayerLoader playerLoader, IHeadToHeadRunner headToHeadRunner)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.playerLoader = playerLoader;
            this.headToHeadRunner = headToHeadRunner;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAll().Reverse());
        }

        [HttpPost]
        public virtual ActionResult RunGame(int playerOneId, int playerTwoId)
        {
            var playerOne = playerRecordsRepository.GetPlayerRecordById(playerOneId);
            var playerTwo = playerRecordsRepository.GetPlayerRecordById(playerTwoId);
            var battleshipsPlayerOne = playerLoader.GetBattleshipsPlayerFromPlayerName(playerOne.Name);
            var battleshipsPlayerTwo = playerLoader.GetBattleshipsPlayerFromPlayerName(playerTwo.Name);
            var result = headToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo);
            return Json(result.Name);
        }
    }
}