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

        public IBattleshipsPlayer FindWinner(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            var playerOneShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerOne);
            var playerTwoShipsPlacement = shipsPlacementFactory.GetShipsPlacement(playerTwo);

            if (!playerOneShipsPlacement.IsValid())
            {
                return playerTwo;
            }

            if (!playerTwoShipsPlacement.IsValid())
            {
                return playerOne;
            }

            while (true)
            {
                MakeMove(playerOne, playerTwo, playerTwoShipsPlacement);
                if (playerTwoShipsPlacement.AllHit())
                {
                    return playerOne;
                }

                MakeMove(playerTwo, playerOne, playerOneShipsPlacement);
                if (playerOneShipsPlacement.AllHit())
                {
                    return playerTwo;
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