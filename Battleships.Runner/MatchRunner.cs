namespace Battleships.Runner
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
            var matchHelper = matchScoreBoardFactory.GetMatchScoreBoard(playerOne, playerTwo);
            var playerOneFirst = true;

            for (var i = 0; i < numberOfRounds; i++)
            {
                var winner = playerOneFirst ? headToHeadRunner.FindWinner(playerOne, playerTwo) : headToHeadRunner.FindWinner(playerTwo, playerOne);
                matchHelper.IncrementWinnerCounter(winner);

                playerOneFirst = !playerOneFirst;
            }

            if (matchHelper.PlayerOneCounter == matchHelper.PlayerTwoCounter)
            {
                matchHelper.IncrementWinnerCounter(headToHeadRunner.FindWinner(playerOne, playerTwo));
            }

            return new MatchResult
                   {
                       Loser = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(matchHelper.GetLoser()),
                       Winner = playerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(matchHelper.GetWinner()),
                       LoserWins = matchHelper.GetLoserCounter(),
                       WinnerWins = matchHelper.GetWinnerCounter(),
                       TimePlayed = DateTime.Now
                   };
        }
    }
}