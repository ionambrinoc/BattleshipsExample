namespace Battleships.Runner.Tests.Models
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class MatchScoreBoardTests
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
        public void PlayerOneWins_incremented_when_playerOne_wins()
        {
            // When
            matchScoreBoard.IncrementPlayerWins(playerOne);
            var winner = matchScoreBoard.GetWinner();
            var winnerWins = matchScoreBoard.GetWinnerWins();

            // Then
            winner.Should().Be(playerOne);
            winnerWins.Should().Be(1);
        }

        [Test]
        public void PlayerTwoWins_incremented_when_playerTwo_wins()
        {
            // When
            matchScoreBoard.IncrementPlayerWins(playerTwo);
            var winner = matchScoreBoard.GetWinner();
            var winnerWins = matchScoreBoard.GetWinnerWins();

            // Then
            winner.Should().Be(playerTwo);
            winnerWins.Should().Be(1);
        }

        [Test]
        public void Winner_is_player_with_greatest_win_count()
        {
            // When
            RunMatchWithPlayerOneAsWinner();
            var winner = matchScoreBoard.GetWinner();

            // Then
            winner.Should().Be(playerOne);
        }

        [Test]
        public void Loser_is_player_with_smallest_win_count()
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
            var winnerCounter = matchScoreBoard.GetWinnerWins();

            // Then
            winnerCounter.Should().Be(2);
        }

        [Test]
        public void Loser_counter_is_the_smallest_counter()
        {
            // When
            RunMatchWithPlayerOneAsWinner();
            var loserCounter = matchScoreBoard.GetLoserWins();

            // Then
            loserCounter.Should().Be(1);
        }

        [Test]
        public void Its_a_draw_if_its_a_draw()
        {
            // When
            SetUpDraw();
            var isDraw = matchScoreBoard.IsDraw();

            // Then
            isDraw.Should().Be(true);
        }

        [Test]
        public void Not_a_draw_if_not_a_draw()
        {
            RunMatchWithPlayerOneAsWinner();
            var isDraw = matchScoreBoard.IsDraw();

            // Then
            isDraw.Should().Be(false);
        }

        private void RunMatchWithPlayerOneAsWinner()
        {
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerTwo);
        }

        private void SetUpDraw()
        {
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerTwo);
        }
    }
}
