namespace Battleships.Player
{
    using System;

    public class PlayerException : Exception
    {
        public PlayerException(string message, Exception innerException, IBattleshipsPlayer player) : base(message, innerException)
        {
            Player = player;
        }

        public IBattleshipsPlayer Player { get; set; }
    }
}
