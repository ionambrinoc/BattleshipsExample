﻿namespace Battleships.Player
{
    using System.Diagnostics;

    public class BattleshipsStopwatch
    {
        private readonly long timeout;
        private readonly Stopwatch stopwatch = new Stopwatch();

        public BattleshipsStopwatch(long timeout)
        {
            this.timeout = timeout;
        }

        public void Start()
        {
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public bool HasTimedOut()
        {
            return stopwatch.ElapsedMilliseconds > timeout;
        }

        public void Reset()
        {
            stopwatch.Reset();
        }
    }
}
