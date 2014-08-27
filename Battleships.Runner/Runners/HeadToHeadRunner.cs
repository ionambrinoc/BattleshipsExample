namespace Battleships.Runner.Runners
{
    using Battleships.Player;
    using Battleships.Runner.Exceptions;
    using Battleships.Runner.Factories;
    using Battleships.Runner.Models;

    public interface IHeadToHeadRunner
    {
        GameResult FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, ILogger logger);
    }

    public class HeadToHeadRunner : IHeadToHeadRunner
    {
        private readonly IShipsPlacementFactory shipsPlacementFactory;
        private bool isPlayerOneTurn;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
        }

        public GameResult FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, ILogger logger)
        {
            try
            {
                var playerOneShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerOne);
                var playerTwoShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerTwo);
                logger.NewGameLog();

                if (!playerOneShipsPlacement.IsValid())
                {
                    var gameResult = new GameResult(playerTwo, ResultType.ShipPositionsInvalid);
                    logger.AddGameResult(gameResult);
                    return gameResult;
                }

                if (!playerTwoShipsPlacement.IsValid())
                {
                    var gameResult = new GameResult(playerOne, ResultType.ShipPositionsInvalid);
                    logger.AddGameResult(gameResult);
                    return gameResult;
                }

                while (true)
                {
                    isPlayerOneTurn = true;
                    MakeMove(playerOne, playerTwo, playerTwoShipsPlacement, logger);

                    if (playerTwoShipsPlacement.AllHit())
                    {
                        var gameResult = new GameResult(playerOne, ResultType.Default);
                        logger.AddGameResult(gameResult);
                        return gameResult;
                    }

                    isPlayerOneTurn = false;
                    MakeMove(playerTwo, playerOne, playerOneShipsPlacement, logger);

                    if (playerOneShipsPlacement.AllHit())
                    {
                        var gameResult = new GameResult(playerTwo, ResultType.Default);
                        logger.AddGameResult(gameResult);
                        return gameResult;
                    }
                }
            }
            catch (OutOfTimeException e)
            {
                var gameResult = new GameResult(e.Winner, ResultType.Timeout);
                logger.AddGameResult(gameResult);
                return gameResult;
            }
            catch (BotException e)
            {
                var gameResult = new GameResult(e.Player == playerOne ? playerTwo : playerOne, ResultType.OpponentThrewException);
                logger.AddGameResult(gameResult);
                return gameResult;
            }
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

        private void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips, ILogger logger)
        {
            var target = attacker.SelectTarget();
            var defendingIsHit = defendingShips.IsHit(target);
            attacker.HandleShotResult(target, defendingIsHit);
            defender.HandleOpponentsShot(target);
            CheckTimeout(attacker, defender);

            logger.AddGameEvent(isPlayerOneTurn, target, defendingIsHit);
        }
    }
}