namespace Battleships.Player
{
    using System;

    public class ShotOffBoardException : Exception
    {
        public ShotOffBoardException(string message, IBattleshipsPlayer player)
        {
            Player = player;
        }

        public IBattleshipsPlayer Player { get; set; }
    }
}