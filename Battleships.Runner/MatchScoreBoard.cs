namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IMatchScoreBoard
    {
        int PlayerOneCounter { get; }
        int PlayerTwoCounter { get; }
        void IncrementWinnerCounter(IBattleshipsPlayer winner);
        IBattleshipsPlayer GetWinner();
        IBattleshipsPlayer GetLoser();
        int GetLoserCounter();
        int GetWinnerCounter();
    }

    public class MatchScoreBoard : IMatchScoreBoard
    {
        private readonly IBattleshipsPlayer playerOne;
        private readonly IBattleshipsPlayer playerTwo;

        public MatchScoreBoard(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
            PlayerOneCounter = 0;
            PlayerTwoCounter = 0;
        }

        public int PlayerOneCounter { get; private set; }
        public int PlayerTwoCounter { get; private set; }

        public void IncrementWinnerCounter(IBattleshipsPlayer winner)
        {
            if (winner == playerOne)
            {
                PlayerOneCounter++;
            }
            else
            {
                PlayerTwoCounter++;
            }
        }

        public IBattleshipsPlayer GetWinner()
        {
            return PlayerOneCounter > PlayerTwoCounter ? playerOne : playerTwo;
        }

        public IBattleshipsPlayer GetLoser()
        {
            return PlayerOneCounter < PlayerTwoCounter ? playerOne : playerTwo;
        }

        public int GetLoserCounter()
        {
            return PlayerOneCounter > PlayerTwoCounter ? PlayerOneCounter : PlayerTwoCounter;
        }

        public int GetWinnerCounter()
        {
            return PlayerOneCounter < PlayerTwoCounter ? PlayerOneCounter : PlayerTwoCounter;
        }
    }
}