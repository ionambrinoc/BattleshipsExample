namespace Battleships.Runner
{
    using Battleships.Player;

    public class HeadToHeadRunner
    {
        private readonly IShipsPlacementFactory shipsPlacementFactory;

        public HeadToHeadRunner(IShipsPlacementFactory shipsPlacementFactory)
        {
            this.shipsPlacementFactory = shipsPlacementFactory;
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

            while (true)
            {
                MakeMove(player1, player2, playerTwoShipsPlacement);
                if (playerTwoShipsPlacement.AllHit())
                {
                    return player1;
                }

                MakeMove(player2, player1, playerOneShipsPlacement);
                if (playerOneShipsPlacement.AllHit())
                {
                    return player2;
                }
            }
        }

        private static void MakeMove(IBattleshipsPlayer attacker, IBattleshipsPlayer defender, IShipsPlacement defendingShips)
        {
            var target = attacker.SelectTarget();

            attacker.HandleShotResult(target, defendingShips.IsHit(target));
            defender.HandleOpponentsShot(target);
        }
    }
}