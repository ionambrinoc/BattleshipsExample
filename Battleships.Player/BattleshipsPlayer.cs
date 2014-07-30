namespace Battleships.Player
{
    using Battleships.Player.Interface;
    using System;
    using System.Collections.Generic;

    public interface IBattleshipsPlayer
    {
        string Name { get; }
        IEnumerable<IShipPosition> GetShipPositions();
        IGridSquare SelectTarget();
        void HandleShotResult(IGridSquare square, bool wasHit);
        void HandleOpponentsShot(IGridSquare square);
        bool HasTimedOut();
        void ResetStopwatch();
    }

    public class BattleshipsPlayer : IBattleshipsPlayer
    {
        private readonly IBattleshipsBot player;
        private readonly BattleshipsStopwatch stopwatch;

        public BattleshipsPlayer(IBattleshipsBot player, long timeout = 1000)
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
            IGridSquare target;
            stopwatch.Start();
            try
            {
                target = player.SelectTarget();
            }
            catch (Exception e)
            {
                throw new PlayerException(String.Format("{0} has thrown an exception while selecting a target", player.Name), e, this);
            }

            stopwatch.Stop();
            return target;
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            stopwatch.Start();
            try
            {
                player.HandleShotResult(square, wasHit);
            }
            catch (Exception e)
            {
                throw new PlayerException(String.Format("{0} has thrown an exception while handling shot result", player.Name), e, this);
            }
            stopwatch.Stop();
        }

        public void HandleOpponentsShot(IGridSquare square)
        {
            stopwatch.Start();
            try
            {
                player.HandleOpponentsShot(square);
            }

            catch (Exception e)
            {
                throw new PlayerException(String.Format("{0} has thrown an exception while handling opponent's shot", player.Name), e, this);
            }
            stopwatch.Stop();
        }

        public bool HasTimedOut()
        {
            return stopwatch.HasTimedOut();
        }

        public void ResetStopwatch()
        {
            stopwatch.Reset();
        }
    }
}
