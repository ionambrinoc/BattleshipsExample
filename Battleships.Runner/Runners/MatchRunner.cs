namespace Battleships.Runner.Runners
{
    using Battleships.Core.Models;
    using Battleships.Player;
    using Battleships.Runner.Factories;
    using System;

    public interface IMatchRunner
    {
        MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds = 100);
    }

    public class MatchRunner : IMatchRunner
    {
        private readonly IMatchScoreBoardFactory matchScoreBoardFactory;
        private readonly IHeadToHeadRunner headToHeadRunner;

        public MatchRunner(IHeadToHeadRunner headToHeadRunner, IMatchScoreBoardFactory matchScoreBoardFactory)
        {
            this.headToHeadRunner = headToHeadRunner;
            this.matchScoreBoardFactory = matchScoreBoardFactory;
        }

        public MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds = 100)
        {
            var logger = new Logger(playerOne, playerTwo);

            playerOne.ResetStopwatch();
            playerTwo.ResetStopwatch();
            var matchScoreBoard = matchScoreBoardFactory.GetMatchScoreBoard(playerOne, playerTwo);
            var playerOneFirst = true;

            for (var i = 0; i < numberOfRounds; i++)
            {
                var winner = playerOneFirst ? headToHeadRunner.FindWinner(playerOne, playerTwo, logger) : headToHeadRunner.FindWinner(playerTwo, playerOne, logger);
                matchScoreBoard.IncrementPlayerWins(winner.Winner);

                playerOneFirst = !playerOneFirst;
            }

            if (matchScoreBoard.IsDraw())
            {
                matchScoreBoard.IncrementPlayerWins(headToHeadRunner.FindWinner(playerOne, playerTwo, logger).Winner);
            }

            logger.CreateTextFile();

            return new MatchResult
                   {
                       Loser = matchScoreBoard.GetLoser().PlayerRecord,
                       Winner = matchScoreBoard.GetWinner().PlayerRecord,
                       LoserWins = matchScoreBoard.GetLoserWins(),
                       WinnerWins = matchScoreBoard.GetWinnerWins(),
                       TimePlayed = DateTime.Now
                   };
        }
    }
}