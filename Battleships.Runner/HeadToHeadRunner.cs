namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IHeadToHeadRunner
    {
        GameResult FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo);
    }

    public class HeadToHeadRunner : IHeadToHeadRunner
    {
        private readonly IShipsPlacementFactory shipsPlacementFactory;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
        }

        public GameResult FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            var playerOneShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerOne);
            var playerTwoShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerTwo);

            if (!playerOneShipsPlacement.IsValid())
            {
                return new GameResult(playerTwo, ResultType.ShipPositionsInvalid);
            }

            if (!playerTwoShipsPlacement.IsValid())
            {
                return new GameResult(playerOne, ResultType.ShipPositionsInvalid);
            }
            try
            {
                while (true)
                {
                    MakeMove(playerOne, playerTwo, playerTwoShipsPlacement);

                    if (playerTwoShipsPlacement.AllHit())
                    {
                        return new GameResult(playerOne, ResultType.Default);
                    }

                    MakeMove(playerTwo, playerOne, playerOneShipsPlacement);

                    if (playerOneShipsPlacement.AllHit())
                    {
                        return new GameResult(playerTwo, ResultType.Default);
                    }
                }
            }
            catch (OutOfTimeException e)
            {
                return new GameResult(e.Winner, ResultType.Timeout);
            }
        }

        private static void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips)
        {
            var target = attacker.SelectTarget();
            var defendingIsHit = defendingShips.IsHit(target);
            attacker.HandleShotResult(target, defendingIsHit);
            defender.HandleOpponentsShot(target);
            CheckTimeout(attacker, defender);
        }

        private static void CheckTimeout(IBattleshipsPlayer attacker, IBattleshipsPlayer defender)
        {
            if (attacker.HasTimedOut())
            {
                throw new OutOfTimeException(defender);
            }
            if (defender.HasTimedOut())
            {
                throw new OutOfTimeException(attacker);
            }
        }
    }
}