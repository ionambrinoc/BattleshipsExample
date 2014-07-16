namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;

    internal class MatchScoreBoardTests
    {
        private IBattleshipsPlayer playerOne;
        private IBattleshipsPlayer playerTwo;
        private IMatchScoreBoard matchScoreBoard;

        [SetUp]
        public void SetUp()
        {
            playerOne = A.Fake<IBattleshipsPlayer>();
            playerTwo = A.Fake<IBattleshipsPlayer>();
            matchScoreBoard = new MatchScoreBoard(playerOne, playerTwo);
        }

        [Test]
        public void PlayerOneCounter_incremented_when_playerOne_wins()
        {
            // When
            matchScoreBoard.IncrementWinnerCounter(playerOne);
            var playerOneCounter = matchScoreBoard.PlayerOneCounter;

            // Then
            playerOneCounter.Should().Be(1);
        }

        [Test]
        public void PlayerTwoCounter_incremented_when_playerTwo_wins()
        {
            // When
            matchScoreBoard.IncrementWinnerCounter(playerTwo);
            var playerTwoCounter = matchScoreBoard.PlayerTwoCounter;

            // Then
            playerTwoCounter.Should().Be(1);
        }

        [Test]
        public void Winner_is_player_with_greatest_counter()
        {
            // When
            RunMatchWithPlayerOneAsWinner();
            var winner = matchScoreBoard.GetWinner();

            // Then
            winner.Should().Be(playerOne);
        }

        [Test]
        public void Loser_is_player_with_smallest_counter()
        {
            // When
            RunMatchWithPlayerOneAsWinner();
            var loser = matchScoreBoard.GetLoser();

            // Then
            loser.Should().Be(playerTwo);
        }

        [Test]
        public void Winner_counter_is_the_greatest_counter()
        {
            // When
            RunMatchWithPlayerOneAsWinner();
            var winnerCounter = matchScoreBoard.GetWinnerCounter();

            // Then
            winnerCounter.Should().Be(2);
        }

        [Test]
        public void Loser_counter_is_the_greatest_counter()
        {
            // When
            RunMatchWithPlayerOneAsWinner();
            var loserCounter = matchScoreBoard.GetLoserCounter();

            // Then
            loserCounter.Should().Be(1);
        }

        private void RunMatchWithPlayerOneAsWinner()
        {
            matchScoreBoard.IncrementWinnerCounter(playerOne);
            matchScoreBoard.IncrementWinnerCounter(playerOne);
            matchScoreBoard.IncrementWinnerCounter(playerTwo);
        }
    }
}