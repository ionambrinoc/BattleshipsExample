namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Models.Players;
    using System;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IPlayerLoader playerLoader;
        private readonly IHeadToHeadRunner headToHeadRunner;
        private readonly IGameResultsRepository gameResultsRepository;

        public PlayersController(IPlayerRecordsRepository playerRecordsRepository, IGameResultsRepository gameResultsRepository, IPlayerLoader playerLoader, IHeadToHeadRunner headToHeadRunner)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.playerLoader = playerLoader;
            this.headToHeadRunner = headToHeadRunner;
            this.gameResultsRepository = gameResultsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAll());
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
                            PlayerOneName = playerRecordsRepository.GetPlayerRecordById(playerOneId).Name,
                            PlayerTwoName = playerRecordsRepository.GetPlayerRecordById(playerTwoId).Name
                        };
            return View(Views.Challenge, model);
        }

        [HttpPost]
        public virtual ActionResult RunGame(int playerOneId, int playerTwoId)
        {
            var playerOne = playerRecordsRepository.GetPlayerRecordById(playerOneId);
            var playerTwo = playerRecordsRepository.GetPlayerRecordById(playerTwoId);
            var battleshipsPlayerOne = playerLoader.GetPlayerFromFile(playerOne.FileName);
            var battleshipsPlayerTwo = playerLoader.GetPlayerFromFile(playerTwo.FileName);
            var result = headToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo);
            var winner = result.Name == playerOne.Name ? playerOne : playerTwo;
            var loser = result.Name == playerOne.Name ? playerTwo : playerOne;
            var gameResult = new GameResult { Winner = winner, Loser = loser, TimePlayed = DateTime.Now };
            gameResultsRepository.Add(gameResult);
            gameResultsRepository.SaveContext();
            return Json(result.Name);
        }
    }
}