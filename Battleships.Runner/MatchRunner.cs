﻿namespace Battleships.Runner
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using System;

    public interface IMatchRunner
    {
        MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds = 100);
    }

    public class MatchRunner : IMatchRunner
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IMatchScoreBoardFactory matchScoreBoardFactory;
        private readonly IHeadToHeadRunner headToHeadRunner;

        public MatchRunner(IHeadToHeadRunner headToHeadRunner, IPlayerRecordsRepository playerRecordsRepository, IMatchScoreBoardFactory matchScoreBoardFactory)
        {
            this.headToHeadRunner = headToHeadRunner;
            this.playerRecordsRepository = playerRecordsRepository;
            this.matchScoreBoardFactory = matchScoreBoardFactory;
        }

        public MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds = 100)
        {
            var matchScoreBoard = matchScoreBoardFactory.GetMatchScoreBoard(playerOne, playerTwo);
            var playerOneFirst = true;

            for (var i = 0; i < numberOfRounds; i++)
            {
                var winner = playerOneFirst ? headToHeadRunner.FindWinner(playerOne, playerTwo) : headToHeadRunner.FindWinner(playerTwo, playerOne);
                matchScoreBoard.IncrementPlayerWins(winner);

                playerOneFirst = !playerOneFirst;
            }

            if (matchScoreBoard.IsDraw())
            {
                matchScoreBoard.IncrementPlayerWins(headToHeadRunner.FindWinner(playerOne, playerTwo));
            }

            return new MatchResult
                   {
                       Loser = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(matchScoreBoard.GetLoser()),
                       Winner = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(matchScoreBoard.GetWinner()),
                       LoserWins = matchScoreBoard.GetLoserWins(),
                       WinnerWins = matchScoreBoard.GetWinnerWins(),
                       TimePlayed = DateTime.Now
                   };
        }
    }
}