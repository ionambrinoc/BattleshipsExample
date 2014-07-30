namespace Battleships.Player.Tests
{
    using NUnit.Framework;
    using System.Threading;

    [TestFixture]
    public class BattleshipsStopwatchTests
    {
        [Test]
        public void Stopwatch_times_out_after_defined_time_has_elapsed()
        {
            //Given
            var battleshipsStopwatch = new BattleshipsStopwatch(1000);

            //When
            battleshipsStopwatch.Start();
            Thread.Sleep(1100);
            battleshipsStopwatch.Stop();

            //Then
            Assert.IsTrue(battleshipsStopwatch.HasTimedOut());
        }

        [Test]
        public void Stopwatch_does_not_time_out_if_defined_time_has_not_yet_elapsed()
        {
            //Given
            var battleshipsStopwatch = new BattleshipsStopwatch(1000);

            //When
            battleshipsStopwatch.Start();
            Thread.Sleep(900);
            battleshipsStopwatch.Stop();

            //Then
            Assert.IsFalse(battleshipsStopwatch.HasTimedOut());
        }

        [Test]
        public void Stopwatch_does_not_reset_when_stopped_and_restarted()
        {
            //Given
            var battleshipsStopwatch = new BattleshipsStopwatch(1000);

            //When
            battleshipsStopwatch.Start();
            Thread.Sleep(1500);
            battleshipsStopwatch.Stop();
            battleshipsStopwatch.Start();

            //Assert
            Assert.IsTrue(battleshipsStopwatch.HasTimedOut());
        }
    }
}
