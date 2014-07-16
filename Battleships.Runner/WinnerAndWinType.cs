namespace Battleships.Runner
{
    using Battleships.Player;

    public enum WinTypes
    {
        Default,
        Timeout,
        Invalid
    };

    public class WinnerAndWinType
    {
        public WinnerAndWinType(IBattleshipsPlayer winner, WinTypes winType)
        {
            Winner = winner;
            WinType = winType;
        }

        public IBattleshipsPlayer Winner { get; set; }
        public WinTypes WinType { get; set; }
    }
}