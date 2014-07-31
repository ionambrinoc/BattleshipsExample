namespace Battleships.Player
{
    using Battleships.Core.Models;
    using Battleships.Player.Interface;
    using System;
    using System.Collections.Generic;

    public interface IBattleshipsPlayer
    {
        string Name { get; }
        PlayerRecord PlayerRecord { get; }
        IEnumerable<IShipPosition> GetShipPositions();
        IGridSquare SelectTarget();
        void HandleShotResult(IGridSquare square, bool wasHit);
        void HandleOpponentsShot(IGridSquare square);
        bool HasTimedOut();
        void ResetStopwatch();
    }

    public class BattleshipsPlayer : IBattleshipsPlayer
    {
        private readonly IBattleshipsBot bot;
        private readonly BattleshipsStopwatch stopwatch;

        public BattleshipsPlayer(IBattleshipsBot bot, PlayerRecord playerRecord, long timeout = 1000)
        {
            this.bot = bot;
            PlayerRecord = playerRecord;
            stopwatch = new BattleshipsStopwatch(timeout);
        }

        public string Name
        {
            get { return PlayerRecord.Name; }
        }

        public PlayerRecord PlayerRecord { get; private set; }

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            stopwatch.Start();
            var positions = bot.GetShipPositions();
            stopwatch.Stop();
            return positions;
        }

        public IGridSquare SelectTarget()
        {
            IGridSquare target;
            stopwatch.Start();
            try
            {
                target = bot.SelectTarget();
            }
            catch (Exception e)
            {
                throw new PlayerException(String.Format("{0} has thrown an exception while selecting a target", bot.Name), e, this);
            }

            stopwatch.Stop();
            return target;
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            stopwatch.Start();
            try
            {
                bot.HandleShotResult(square, wasHit);
            }
            catch (Exception e)
            {
                throw new PlayerException(String.Format("{0} has thrown an exception while handling shot result", bot.Name), e, this);
            }
            stopwatch.Stop();
        }

        public void HandleOpponentsShot(IGridSquare square)
        {
            stopwatch.Start();
            try
            {
                bot.HandleOpponentsShot(square);
            }

            catch (Exception e)
            {
                throw new PlayerException(String.Format("{0} has thrown an exception while handling opponent's shot", bot.Name), e, this);
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
