namespace Battleships.Player
{
    using System.Collections.Generic;

    public interface IBattleshipsPlayer
    {
        string Name { get; }
        IEnumerable<IShipPosition> GetShipPositions();
        IGridSquare SelectTarget();
        void HandleShotResult(IGridSquare square, bool wasHit);
        void HandleOpponentsShot(IGridSquare square);
        bool HasTimedOut();
    }

    public class BattleshipsPlayer : IBattleshipsPlayer
    {
        private readonly IBattleshipsBot player;
        private readonly BattleshipsStopwatch stopwatch;

        public BattleshipsPlayer(IBattleshipsBot player, long timeout = 10000)
        {
            this.player = player;
            stopwatch = new BattleshipsStopwatch(timeout);
        }

        public string Name
        {
            get { return player.Name; }
        }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            stopwatch.Start();
            var positions = player.GetShipPositions();
            stopwatch.Stop();
            return positions;
        }

        public IGridSquare SelectTarget()
        {
            stopwatch.Start();
            var target = player.SelectTarget();
            stopwatch.Stop();
            return target;
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            stopwatch.Start();
            player.HandleShotResult(square, wasHit);
            stopwatch.Stop();
        }

        public void HandleOpponentsShot(IGridSquare square)
        {
            stopwatch.Start();
            player.HandleOpponentsShot(square);
            stopwatch.Stop();
        }

        public bool HasTimedOut()
        {
            return stopwatch.HasTimedOut();
        }
    }
}