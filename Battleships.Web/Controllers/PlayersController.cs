﻿namespace Battleships.Web.Controllers
{
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public partial class PlayersController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IHeadToHeadRunner headToHeadRunner;
        private readonly IMatchResultsRepository matchResultsRepository;

        public PlayersController(IPlayerRecordsRepository playerRecordsRepository, IMatchResultsRepository matchResultsRepository,
                                 IHeadToHeadRunner headToHeadRunner)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.headToHeadRunner = headToHeadRunner;
            this.matchResultsRepository = matchResultsRepository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(playerRecordsRepository.GetAll().Reverse());
        }

        [HttpPost]
        public virtual ActionResult RunGame(int playerOneId, int playerTwoId)
        {
            var battleshipsPlayerOne = playerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(playerOneId);
            var battleshipsPlayerTwo = playerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(playerTwoId);
            var winnerAndWinType = headToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo);
            var winner = winnerAndWinType.Winner;
            var loser = winner == battleshipsPlayerOne ? battleshipsPlayerTwo : battleshipsPlayerOne;
            var matchResult = new MatchResult
                              {
                                  WinnerId = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(winner).Id,
                                  LoserId = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(loser).Id,
                                  WinnerWins = 1,
                                  LoserWins = 0,
                                  TimePlayed = DateTime.Now
                              };
            matchResultsRepository.Add(matchResult);
            matchResultsRepository.SaveContext();
            return Json(new { winnerName = winner.Name, winType = (int)winnerAndWinType.WinType });
        }
    }
}