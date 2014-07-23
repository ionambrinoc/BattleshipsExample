namespace Battleships.Runner.Exceptions
{
    using Battleships.Player;
    using System;

    public class OutOfTimeException : Exception
    {
        public OutOfTimeException(IBattleshipsPlayer winner)
        {
            Winner = winner;
        }

        public IBattleshipsPlayer Winner { get; set; }
    }
}
