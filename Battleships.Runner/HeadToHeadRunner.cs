namespace Battleships.Runner
{
    using Battleships.Player;

    public class HeadToHeadRunner
    {
        private readonly IShipPositionValidator shipPositionValidator;

        public HeadToHeadRunner(IShipPositionValidator shipPositionValidator)
        {
            this.shipPositionValidator = shipPositionValidator;
        }

        public IBattleshipsPlayer RunGame(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            return player1;
        }
    }
}