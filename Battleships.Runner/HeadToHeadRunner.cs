namespace Battleships.Runner
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using System;

    public interface IHeadToHeadRunner
    {
        IBattleshipsPlayer FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo);
        MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds);
    }

    public class HeadToHeadRunner : IHeadToHeadRunner
    {
        private readonly IShipsPlacementFactory shipsPlacementFactory;
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IMatchHelperFactory matchHelperFactory;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory, IMatchHelperFactory matchHelperFactory, IPlayerRecordsRepository playerRecordsRepository)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
            this.playerRecordsRepository = playerRecordsRepository;
            this.matchHelperFactory = matchHelperFactory;
        }

        public IBattleshipsPlayer FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            var playerOneShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerOne);
            var playerTwoShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerTwo);

            if (!playerOneShipsPlacement.IsValid())
            {
                return playerTwo;
            }

            if (!playerTwoShipsPlacement.IsValid())
            {
                return playerOne;
            }

            while (true)
            {
                MakeMove(playerOne, playerTwo, playerTwoShipsPlacement);
                if (playerTwoShipsPlacement.AllHit())
                {
                    return playerOne;
                }

                MakeMove(playerTwo, playerOne, playerOneShipsPlacement);
                if (playerOneShipsPlacement.AllHit())
                {
                    return playerTwo;
                }
            }
        }

        public MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds = 100)
        {
            var matchHelper = matchHelperFactory.GetMatchHelper(playerOne, playerTwo);
            var playerOneFirst = true;

            for (var i = 0; i < numberOfRounds; i++)
            {
                var winner = playerOneFirst ? FindWinner(playerOne, playerTwo) : FindWinner(playerTwo, playerOne);
                matchHelper.IncrementWinnerCounter(winner);

                playerOneFirst = !playerOneFirst;
            }

            if (matchHelper.PlayerOneCounter == matchHelper.PlayerTwoCounter)
            {
                matchHelper.IncrementWinnerCounter(FindWinner(playerOne, playerTwo));
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

        private static void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips)
        {
            var target = attacker.SelectTarget();

            attacker.HandleShotResult(target, defendingShips.IsHit(target));
            defender.HandleOpponentsShot(target);
        }
    }
}