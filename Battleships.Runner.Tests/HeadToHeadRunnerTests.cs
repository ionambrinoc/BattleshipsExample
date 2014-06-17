namespace Battleships.Runner.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class HeadToHeadRunnerTests
    {
        [Test]
        public void Player_loses_if_ship_positions_invalid() {}

        [Test]
        public void Player_loses_if_it_throws_exception() {}

        [Test]
        public void Player_wins_when_all_opponents_ships_are_sunk() {}

        [Test]
        public void Player_loses_on_timeout() {}

        [Test]
        public void Shot_result_is_reported_correctly_to_player() {}

        [Test]
        public void Opponent_shot_is_reported_correctly_to_player() {}
    }
}