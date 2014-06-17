namespace Battleships.Runner
{
    using Battleships.Player;
    using System;

    public class HeadToHeadRunner
    {
        public IBattleshipsPlayer RunGame(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            var random = new Random();
            return random.Next(2) == 0 ? player1 : player2;
        }
    }
}