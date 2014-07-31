namespace Battleships.Player
{
    using System;

    public class BotException : Exception
    {
        public BotException(string message, Exception innerException, IBattleshipsPlayer player) : base(message, innerException)
        {
            Player = player;
        }

        public IBattleshipsPlayer Player { get; set; }
    }
}
