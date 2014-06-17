namespace Battleships.Runner
{
    using Battleships.Player;
    using System;

    public class HeadToHead
    {
        private readonly IBattleshipsPlayer player1;
        private readonly IBattleshipsPlayer player2;

        public HeadToHead(IBattleshipsPlayer player1, IBattleshipsPlayer player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }

        public IBattleshipsPlayer RunGame()
        {
            var random = new Random();
            return random.Next(2) == 0 ? player1 : player2;
        }
    }
}