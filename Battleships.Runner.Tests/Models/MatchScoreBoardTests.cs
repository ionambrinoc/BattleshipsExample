namespace Battleships.Runner.Tests.Models
{
    using Battleships.Player;
    using Battleships.Runner.Factories;
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
            var factory = new MatchScoreBoardFactory();
            matchScoreBoard = factory.GetMatchScoreBoard(playerOne, playerTwo);
        }

        [Test]
        public void Can_increment_wins()
        {
            // When
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerOne);

            // Then
            matchScoreBoard.GetWinnerWins().Should().Be(2);
        }

        [Test]
        public void Number_of_wins_is_zero_if_player_has_not_won_anything()
        {
            // When
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerOne);

            // Then
            matchScoreBoard.GetLoserWins().Should().Be(0);
        }

        [Test]
        public void Winner_and_loser_are_players_with_higher_and_lower_win_counts_respectively()
        {
            // Given
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerTwo);
            matchScoreBoard.IncrementPlayerWins(playerTwo);

            // When
            var winner = matchScoreBoard.GetWinner();
            var loser = matchScoreBoard.GetLoser();

            // Then
            winner.Should().Be(playerTwo);
            loser.Should().Be(playerOne);
        }

        [Test]
        public void Is_a_draw_if_player_wins_are_equal()
        {
            // Given
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerTwo);

            // When
            var isDraw = matchScoreBoard.IsDraw();

            // Then
            isDraw.Should().Be(true);
        }

        [Test]
        public void Is_not_a_draw_if_player_wins_are_not_equal()
        {
            // Given
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerOne);
            matchScoreBoard.IncrementPlayerWins(playerTwo);

            // When
            var isDraw = matchScoreBoard.IsDraw();

            // Then
            isDraw.Should().Be(false);
        }
    }
}
