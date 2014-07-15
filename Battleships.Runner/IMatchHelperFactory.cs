namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IMatchHelperFactory
    {
        IMatchHelper GetMatchHelper(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo);
    }

    public class MatchHelperFactory : IMatchHelperFactory
    {
        public IMatchHelper GetMatchHelper(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            return new MatchHelper(playerOne, playerTwo);
        }
    }
}