namespace Battleships.Runner.Runners
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Runner.Exceptions;
    using Battleships.Runner.Factories;
    using Battleships.Runner.Models;
    using System;

    public interface IHeadToHeadRunner
    {
        GameResult FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, IGameLogFactory gameLogFactory, IGameLogRepository gameLogRepo);
    }

    public class HeadToHeadRunner : IHeadToHeadRunner
    {
        private readonly IShipsPlacementFactory shipsPlacementFactory;
        private bool isPlayerOneTurn;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
        }

        public GameResult FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, IGameLogFactory gameLogFactory, IGameLogRepository gameLogRepo)
        {
            try
            {
                var playerOneShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerOne);
                var playerTwoShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerTwo);

                gameLogFactory.InitialiseGameLog(playerOne.PlayerRecord, playerTwo.PlayerRecord, DateTime.Now, playerOne.GetShipPositions(), playerTwo.GetShipPositions());

                if (!playerOneShipsPlacement.IsValid())
                {
                    gameLogRepo.AddGameLog(gameLogFactory.GetCompleteGame(false, ResultType.ShipPositionsInvalid));
                    gameLogRepo.SaveContext();
                    return new GameResult(playerTwo, ResultType.ShipPositionsInvalid);
                }

                if (!playerTwoShipsPlacement.IsValid())
                {
                    gameLogRepo.AddGameLog(gameLogFactory.GetCompleteGame(true, ResultType.ShipPositionsInvalid));
                    gameLogRepo.SaveContext();
                    return new GameResult(playerOne, ResultType.ShipPositionsInvalid);
                }

                while (true)
                {
                    isPlayerOneTurn = true;
                    MakeMove(playerOne, playerTwo, playerTwoShipsPlacement, gameLogFactory);

                    if (playerTwoShipsPlacement.AllHit())
                    {
                        gameLogRepo.AddGameLog(gameLogFactory.GetCompleteGame(true, ResultType.Default));
                        gameLogRepo.SaveContext();
                        return new GameResult(playerOne, ResultType.Default);
                    }

                    isPlayerOneTurn = false;
                    MakeMove(playerTwo, playerOne, playerOneShipsPlacement, gameLogFactory);

                    if (playerOneShipsPlacement.AllHit())
                    {
                        gameLogRepo.AddGameLog(gameLogFactory.GetCompleteGame(false, ResultType.Default));
                        gameLogRepo.SaveContext();
                        return new GameResult(playerTwo, ResultType.Default);
                    }
                }
            }
            catch (OutOfTimeException e)
            {
                gameLogRepo.AddGameLog(gameLogFactory.GetCompleteGame(isPlayerOneTurn, ResultType.Timeout));
                gameLogRepo.SaveContext();
                return new GameResult(e.Winner, ResultType.Timeout);
            }
            catch (BotException e)
            {
                gameLogRepo.AddGameLog(gameLogFactory.GetCompleteGame(!isPlayerOneTurn, ResultType.OpponentThrewException));
                gameLogRepo.SaveContext();
                return new GameResult(e.Player == playerOne ? playerTwo : playerOne, ResultType.OpponentThrewException);
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

        private void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips, IGameLogFactory gameLogFactory)
        {
            var target = attacker.SelectTarget();
            var defendingIsHit = defendingShips.IsHit(target);
            attacker.HandleShotResult(target, defendingIsHit);
            defender.HandleOpponentsShot(target);
            CheckTimeout(attacker, defender);

            gameLogFactory.AddGameEvent(DateTime.Now, isPlayerOneTurn, target, defendingIsHit);
        }
    }
}