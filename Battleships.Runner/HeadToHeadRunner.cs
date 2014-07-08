namespace Battleships.Runner
{
    using Battleships.Player;

    public class HeadToHeadRunner
    {
        private readonly IShipPositionValidator shipPositionValidator;
        private readonly IMoveCheckerFactory moveCheckerFactory;

        public HeadToHeadRunner(IShipPositionValidator shipPositionValidator, IMoveCheckerFactory moveCheckerFactory)
        {
            this.shipPositionValidator = shipPositionValidator;
            this.moveCheckerFactory = moveCheckerFactory;
        }

        public IBattleshipsPlayer RunGame(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            var winnerByDefault = ValidateStartingShipsPositions(player1, player2);
            if (winnerByDefault != null)
            {
                return winnerByDefault;
            }

            var moveCheckerForPlayer1 = moveCheckerFactory.GetMoveChecker(player2.GetShipPositions());
            var moveCheckerForPlayer2 = moveCheckerFactory.GetMoveChecker(player1.GetShipPositions());

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

        private IBattleshipsPlayer ValidateStartingShipsPositions(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            try
            {
                if (!shipPositionValidator.IsValid(player1.GetShipPositions()))
                {
                    return player2;
                }
            }
            catch
            {
                return player2;
            }

            try
            {
                if (!shipPositionValidator.IsValid(player2.GetShipPositions()))
                {
                    return player1;
                }
            }
            catch
            {
                return player1;
            }

            return null;
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