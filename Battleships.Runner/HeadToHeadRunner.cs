namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Linq;

    public class HeadToHeadRunner
    {
        private readonly IShipPositionValidator shipPositionValidator;

        public HeadToHeadRunner(IShipPositionValidator shipPositionValidator)
        {
            this.shipPositionValidator = shipPositionValidator;
        }

        public IBattleshipsPlayer RunGame(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            var moveCheckerForPlayer1 = new MoveChecker(player2.GetShipPositions().ToList());
            var moveCheckerForPlayer2 = new MoveChecker(player1.GetShipPositions().ToList());

            var winnerByDefault = ValidateStartingShipsPositions(player1, player2);
            if (winnerByDefault != null)
            {
                return winnerByDefault;
            }

            var isFinished = false;
            var playerTurn = 1;
            while (!isFinished)
            {
                playerTurn = 1 - playerTurn;
                isFinished = playerTurn == 0 ? MakeMove(player1, player2, moveCheckerForPlayer1) : MakeMove(player2, player1, moveCheckerForPlayer2);
            }

            if (playerTurn == 0)
            {
                return player1;
            }
            return player2;
        }

        private IBattleshipsPlayer ValidateStartingShipsPositions(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            if (!shipPositionValidator.IsValid(player1.GetShipPositions()))
            {
                return player2;
            }

            if (!shipPositionValidator.IsValid(player2.GetShipPositions()))
            {
                return player1;
            }
            return null;
        }

        private bool MakeMove(IBattleshipsPlayer attackingPlayer, IBattleshipsPlayer opposingPlayer, MoveChecker moveChecker)
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