namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IHeadToHeadRunner
    {
        WinnerAndWinType FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo);
    }

    public class HeadToHeadRunner : IHeadToHeadRunner
    {
        private readonly IShipsPlacementFactory shipsPlacementFactory;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
        }

        public WinnerAndWinType FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            var playerOneShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerOne);
            var playerTwoShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerTwo);

            if (!playerOneShipsPlacement.IsValid())
            {
                return new WinnerAndWinType(playerTwo, WinTypes.Invalid);
            }

            if (!playerTwoShipsPlacement.IsValid())
            {
                return new WinnerAndWinType(playerOne, WinTypes.Invalid);
            }

            while (true)
            {
                MakeMove(playerOne, playerTwo, playerTwoShipsPlacement);

                if (playerOne.HasTimedOut())
                {
                    return new WinnerAndWinType(playerTwo, WinTypes.Timeout);
                }
                if (playerTwo.HasTimedOut())
                {
                    return new WinnerAndWinType(playerOne, WinTypes.Timeout);
                }

                if (playerTwoShipsPlacement.AllHit())
                {
                    return new WinnerAndWinType(playerOne, WinTypes.Default);
                }

                MakeMove(playerTwo, playerOne, playerOneShipsPlacement);

                if (playerTwo.HasTimedOut())
                {
                    return new WinnerAndWinType(playerOne, WinTypes.Timeout);
                }
                if (playerOne.HasTimedOut())
                {
                    return new WinnerAndWinType(playerTwo, WinTypes.Timeout);
                }

                if (playerOneShipsPlacement.AllHit())
                {
                    return new WinnerAndWinType(playerTwo, WinTypes.Default);
                }
            }
        }

        private static void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips)
        {
            var target = attacker.SelectTarget();
            var defendingIsHit = defendingShips.IsHit(target);
            attacker.HandleShotResult(target, defendingIsHit);
            defender.HandleOpponentsShot(target);
        }
    }
}