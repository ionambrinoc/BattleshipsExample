namespace Battleships.Runner
{
    using Battleships.Player;
    using Battleships.Runner.Models;

    public interface IHeadToHeadRunner
    {
        IBattleshipsPlayer FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo);
        MatchResult GetMatchResult(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, int numberOfRounds);
    }

    public class HeadToHeadRunner : IHeadToHeadRunner
    {
        private readonly IShipsPlacementFactory shipsPlacementFactory;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
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
            var playerOneWinCount = 0;
            var playerTwoWinCount = 0;
            var playerOneFirst = true;

            for (var i = 0; i < numberOfRounds; i++)
            {
                var winner = playerOneFirst ? FindWinner(playerOne, playerTwo) : FindWinner(playerTwo, playerOne);
                if (winner == playerOne)
                {
                    playerOneWinCount++;
                }
                else
                {
                    playerTwoWinCount++;
                }

                playerOneFirst = !playerOneFirst;
            }

            IBattleshipsPlayer matchWinner;
            if (playerOneWinCount == playerTwoWinCount)
            {
                matchWinner = FindWinner(playerOne, playerTwo);
            }
            else
            {
                matchWinner = playerOneWinCount < playerTwoWinCount ? playerTwo : playerOne;
            }

            var matchLoser = matchWinner == playerOne ? playerTwo : playerOne;

            return new MatchResult();
        }

        private static void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips)
        {
            var target = attacker.SelectTarget();

            attacker.HandleShotResult(target, defendingShips.IsHit(target));
            defender.HandleOpponentsShot(target);
        }
    }
}