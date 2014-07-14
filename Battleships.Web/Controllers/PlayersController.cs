namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Models.Players;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayersRepository playersRepository;
        private readonly IPlayerLoader playerLoader;
        private readonly IHeadToHeadRunner headToHeadRunner;

        public PlayersController(IPlayersRepository playersRepository, IPlayerLoader playerLoader, IHeadToHeadRunner headToHeadRunner)
        {
            this.playersRepository = playersRepository;
            this.playerLoader = playerLoader;
            this.headToHeadRunner = headToHeadRunner;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playersRepository.GetAll());
        }

        [HttpPost]
        public virtual ActionResult Index(FormCollection form)
        {
            return RedirectToAction(Actions.Index());
        }

        [HttpGet]
        //[Route("{playerOneName}/vs/{PlayerTwoName}")]
        public virtual ActionResult Challenge(int playerOneId, int playerTwoId)
        {
            var model = new ChallengeViewModel
                        {
                            PlayerOneId = playerOneId,
                            PlayerTwoId = playerTwoId,
                            PlayerOneName = playersRepository.GetPlayerById(playerOneId).Name,
                            PlayerTwoName = playersRepository.GetPlayerById(playerTwoId).Name
                        };
            return View(Views.Challenge, model);
        }

        [HttpPost]
        public virtual ActionResult RunGame(int playerOneId, int playerTwoId)
        {
            var playerOne = playersRepository.GetPlayerById(playerOneId);
            var playerTwo = playersRepository.GetPlayerById(playerTwoId);
            var battleshipsPlayerOne = playerLoader.GetPlayerFromFile(playerOne.FileName);
            var battleshipsPlayerTwo = playerLoader.GetPlayerFromFile(playerTwo.FileName);
            var result = headToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo);
            return Json(result.Name);
        }
    }
}