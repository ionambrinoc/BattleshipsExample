namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IMatchScoreBoardFactory
    {
        IMatchScoreBoard GetMatchScoreBoard(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo);
    }

    public class MatchScoreBoardFactory : IMatchScoreBoardFactory
    {
        public IMatchScoreBoard GetMatchScoreBoard(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            return new MatchScoreBoard(playerOne, playerTwo);
        }
    }
}