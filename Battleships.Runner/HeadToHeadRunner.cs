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

            var isFinished = false;
            var playerTurn = 0;
            while (!isFinished)
            {
                if (playerTurn == 0)
                {
                    isFinished = MakeMove(player1, player2, player1ShipPositions, player2ShipPositions);
                }
                else
                {
                    isFinished = MakeMove(player2, player1, player2ShipPositions, player1ShipPositions);
                }
                playerTurn = 1 - playerTurn;
            }
        }

        private bool MakeMove(IBattleshipsPlayer attackingPlayer, IBattleshipsPlayer opposingPlayer, IEnumerable<IShipPosition> playerAShipPositions, IEnumerable<IShipPosition> playerOShipPositions)
        {
            var target = attackingPlayer.SelectTarget();
            var moveChecker = new MoveChecker(playerOShipPositions, target);
            var result = moveChecker.CheckResultOfMove();

            attackingPlayer.HandleShotResult(target, result);
            opposingPlayer.HandleOpponentsShot(target);
        }
    }
}