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
            var playerOneBattleshipStopwatch = new BattleshipsStopwatch();
            var playerTwoBattleshipStopwatch = new BattleshipsStopwatch();

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
                MakeMove(playerOne, playerTwo, playerTwoShipsPlacement, playerOneBattleshipStopwatch, playerTwoBattleshipStopwatch);

                if (playerOneBattleshipStopwatch.HasTimedOut())
                {
                    return new WinnerAndWinType(playerTwo, WinTypes.Timeout);
                }
                if (playerTwoBattleshipStopwatch.HasTimedOut())
                {
                    return new WinnerAndWinType(playerOne, WinTypes.Timeout);
                }

                if (playerTwoShipsPlacement.AllHit())
                {
                    return new WinnerAndWinType(playerOne, WinTypes.Default);
                }

                MakeMove(playerTwo, playerOne, playerOneShipsPlacement, playerTwoBattleshipStopwatch, playerOneBattleshipStopwatch);

                if (playerTwoBattleshipStopwatch.HasTimedOut())
                {
                    return new WinnerAndWinType(playerOne, WinTypes.Timeout);
                }
                if (playerOneBattleshipStopwatch.HasTimedOut())
                {
                    return new WinnerAndWinType(playerTwo, WinTypes.Timeout);
                }

                if (playerOneShipsPlacement.AllHit())
                {
                    return new WinnerAndWinType(playerTwo, WinTypes.Default);
                }
            }
        }

        private static void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips, BattleshipsStopwatch attackerStopwatch, BattleshipsStopwatch defenderStopwatch)
        {
            attackerStopwatch.Start();
            var target = attacker.SelectTarget();
            attackerStopwatch.Stop();

            var defendingIsHit = defendingShips.IsHit(target);

            attackerStopwatch.Start();
            attacker.HandleShotResult(target, defendingIsHit);
            attackerStopwatch.Stop();

            defenderStopwatch.Start();
            defender.HandleOpponentsShot(target);
            defenderStopwatch.Stop();
        }
    }
}