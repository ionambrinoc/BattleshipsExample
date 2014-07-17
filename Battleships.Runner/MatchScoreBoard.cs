namespace Battleships.Runner
{
    using Battleships.Player;

    public interface IMatchScoreBoard
    {
        void IncrementPlayerWins(IBattleshipsPlayer winner);
        IBattleshipsPlayer GetWinner();
        IBattleshipsPlayer GetLoser();
        int GetLoserWins();
        int GetWinnerWins();
        bool IsDraw();
    }

    public class MatchScoreBoard : IMatchScoreBoard
    {
        private readonly IBattleshipsPlayer playerOne;
        private readonly IBattleshipsPlayer playerTwo;
        private int playerOneWins;
        private int playerTwoWins;

        public MatchScoreBoard(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
        }

        public void IncrementPlayerWins(IBattleshipsPlayer winner)
        {
            if (winner == playerOne)
            {
                playerOneWins++;
            }
            else
            {
                playerTwoWins++;
            }
        }

        public IBattleshipsPlayer GetWinner()
        {
            return playerOneWins > playerTwoWins ? playerOne : playerTwo;
        }

        public IBattleshipsPlayer GetLoser()
        {
            return playerOneWins < playerTwoWins ? playerOne : playerTwo;
        }

        public int GetLoserWins()
        {
            return playerOneWins < playerTwoWins ? playerOneWins : playerTwoWins;
        }

        public int GetWinnerWins()
        {
            return playerOneWins > playerTwoWins ? playerOneWins : playerTwoWins;
        }

        public bool IsDraw()
        {
            return playerOneWins == playerTwoWins;
        }
    }
}