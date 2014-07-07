namespace Battleships.Runner
{
    using Battleships.Player;
    using System.Collections.Generic;

    public class HeadToHeadRunner
    {
        private readonly IShipPositionValidator shipPositionValidator;

        public HeadToHeadRunner(IShipPositionValidator shipPositionValidator)
        {
            this.shipPositionValidator = shipPositionValidator;
        }

        public IBattleshipsPlayer RunGame(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            var player1ShipPositions = player1.GetShipPositions();
            var player2ShipPositions = player2.GetShipPositions();
            var cellsHitByPlayer1 = new CellsHitByPlayerChecker();
            var cellsHitByPlayer2 = new CellsHitByPlayerChecker();

            if (!shipPositionValidator.IsValid(player1ShipPositions))
            {
                return player2;
            }

            if (!shipPositionValidator.IsValid(player2ShipPositions))
            {
                return player1;
            }

            var isFinished = false;
            var playerTurn = 1;
            while (!isFinished)
            {
                playerTurn = 1 - playerTurn;
                if (playerTurn == 0)
                {
                    isFinished = MakeMove(player1, player2, player2ShipPositions, cellsHitByPlayer1);
                }
                else
                {
                    isFinished = MakeMove(player2, player1, player1ShipPositions, cellsHitByPlayer2);
                }
            }

            if (playerTurn == 0)
            {
                return player2;
            }
            return player2;
        }

        private bool MakeMove(IBattleshipsPlayer attackingPlayer, IBattleshipsPlayer opposingPlayer,
                              IEnumerable<IShipPosition> playerOShipPositions, CellsHitByPlayerChecker cellsHitByAttackingPlayer)
        {
            var target = attackingPlayer.SelectTarget();
            var moveChecker = new MoveChecker(playerOShipPositions, target);
            var HasHit = moveChecker.CheckResultOfMove();

            if (HasHit)
            {
                cellsHitByAttackingPlayer.AddCell(target);
            }

            attackingPlayer.HandleShotResult(target, HasHit);
            opposingPlayer.HandleOpponentsShot(target);

            return cellsHitByAttackingPlayer.AllHit();
        }
    }
}