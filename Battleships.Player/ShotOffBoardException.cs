namespace Battleships.Player
{
    using System;

    public class ShotOffBoardException : Exception
    {
        public ShotOffBoardException(string message, IBattleshipsPlayer player) : base(message)
        {
            Player = player;
        }

        public IBattleshipsPlayer Player { get; private set; }
    }
}