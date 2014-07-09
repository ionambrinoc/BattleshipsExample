namespace Battleships.Runner
{
    using Battleships.Player;

    public class HeadToHeadRunner
    {
        private readonly IMoveCheckerFactory moveCheckerFactory;
        private readonly IShipsPlacementFactory shipsPlacementFactory;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory, IMoveCheckerFactory moveCheckerFactory)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
            this.moveCheckerFactory = moveCheckerFactory;
        }

        public IBattleshipsPlayer RunGame(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            var playerOneShipsPlacement = shipsPlacementFactory.GetShipPlacement(player1);
            var playerTwoShipsPlacement = shipsPlacementFactory.GetShipPlacement(player2);

            if (!playerOneShipsPlacement.IsValid())
            {
                return player2;
            }

            if (!playerTwoShipsPlacement.IsValid())
            {
                return player1;
            }

            var moveCheckerForPlayer1 = moveCheckerFactory.GetMoveChecker(playerTwoShipsPlacement);
            var moveCheckerForPlayer2 = moveCheckerFactory.GetMoveChecker(playerOneShipsPlacement);

            var isFinished = false;
            var playerOnesTurn = false;
            while (!isFinished)
            {
                playerOnesTurn = !playerOnesTurn;
                isFinished = playerOnesTurn ? MakeMove(player1, player2, moveCheckerForPlayer1) : MakeMove(player2, player1, moveCheckerForPlayer2);
            }

            if (playerOnesTurn)
            {
                return player1;
            }
            return player2;
        }

        private bool MakeMove(IBattleshipsPlayer attackingPlayer, IBattleshipsPlayer opposingPlayer, IMoveChecker moveChecker)
        {
            var target = attackingPlayer.SelectTarget();
            var hasHit = moveChecker.CheckResultOfMove(target);

            if (hasHit)
            {
                moveChecker.AddToCellsHit(target);
            }

            attackingPlayer.HandleShotResult(target, hasHit);
            opposingPlayer.HandleOpponentsShot(target);

            return moveChecker.AllHit();
        }
    }
}