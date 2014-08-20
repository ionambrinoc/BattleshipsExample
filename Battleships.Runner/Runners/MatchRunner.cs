namespace Battleships.Runner.Runners
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
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
        private readonly IGameLogFactory gameLogFactory;
        private readonly IGameLogRepository gameLogRepository;


        public MatchRunner(IHeadToHeadRunner headToHeadRunner, IMatchScoreBoardFactory matchScoreBoardFactory, IGameLogFactory gameLogFactory, IGameLogRepository gameLogRepository)
        {
            this.headToHeadRunner = headToHeadRunner;
            this.matchScoreBoardFactory = matchScoreBoardFactory;
            this.gameLogFactory = gameLogFactory;
            this.gameLogRepository = gameLogRepository;
        }

        public MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds = 100)
        {
            playerOne.ResetStopwatch();
            playerTwo.ResetStopwatch();
            var matchScoreBoard = matchScoreBoardFactory.GetMatchScoreBoard(playerOne, playerTwo);
            var playerOneFirst = true;

            for (var i = 0; i < numberOfRounds; i++)
            {
                var winner = playerOneFirst ? headToHeadRunner.FindWinner(playerOne, playerTwo, gameLogFactory, gameLogRepository) : headToHeadRunner.FindWinner(playerTwo, playerOne, gameLogFactory, gameLogRepository);
                matchScoreBoard.IncrementPlayerWins(winner.Winner);

                playerOneFirst = !playerOneFirst;
            }

            if (matchScoreBoard.IsDraw())
            {
                matchScoreBoard.IncrementPlayerWins(headToHeadRunner.FindWinner(playerOne, playerTwo, gameLogFactory, gameLogRepository).Winner);
            }

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
