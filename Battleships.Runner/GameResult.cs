namespace Battleships.Runner
{
    using Battleships.Player;

    public enum ResultType
    {
        Default,
        Timeout,
        ShipPositionsInvalid
    };

    public class GameResult
    {
        public GameResult(IBattleshipsPlayer winner, ResultType resultType)
        {
            Winner = winner;
            ResultType = resultType;
        }

        public IBattleshipsPlayer Winner { get; set; }
        public ResultType ResultType { get; set; }
    }
}
