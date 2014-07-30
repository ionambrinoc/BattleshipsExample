namespace Battleships.Runner.Factories
{
    using Battleships.Player;
    using Battleships.Runner.Models;

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
