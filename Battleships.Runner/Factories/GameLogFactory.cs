namespace Battleships.Runner.Factories
{
    using Battleships.Core.Models;
    using Battleships.Player.Interface;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public interface IGameLogFactory
    {
        void InitialiseGameLog(PlayerRecord player1, PlayerRecord player2, DateTime startTime, IEnumerable<IShipPosition> player1Positions, IEnumerable<IShipPosition> player2Positions);
        void AddGameEvent(DateTime time, bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit);
        GameLog GetCompleteGame(bool player1Won, ResultType resultType);
    }

    public class GameLogFactory : IGameLogFactory
    {
        private readonly GridSquareStringConverter converter;
        private GameLog gameLog;

        public GameLogFactory(GridSquareStringConverter converter)
        {
            this.converter = converter;
        }

        public void InitialiseGameLog(PlayerRecord player1, PlayerRecord player2, DateTime startTime, IEnumerable<IShipPosition> player1Positions, IEnumerable<IShipPosition> player2Positions)
        {
            gameLog = new GameLog
                      {
                          Player1 = player1,
                          Player2 = player2,
                          StartTime = startTime,
                          Player1PositionsString = player1Positions.Aggregate("", (s, position) => s + converter.ShipPositionToString(position)),
                          Player2PositionsString = player2Positions.Aggregate("", (s, position) => s + converter.ShipPositionToString(position)),
                          DetailedLog = new Collection<GameEvent>()
                      };
        }

        public void AddGameEvent(DateTime time, bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit)
        {
            var selectedTargetString = converter.GridSquareToString(selectedTarget);
            var gameEvent = new GameEvent
                            {
                                IsHit = isHit,
                                IsPlayer1Turn = isPlayer1Turn,
                                SelectedTarget = selectedTargetString,
                                Time = time
                            };
            gameLog.DetailedLog.Add(gameEvent);
        }

        public GameLog GetCompleteGame(bool player1Won, ResultType resultType)
        {
            gameLog.Player1Won = player1Won;
            gameLog.ResultType = resultType;
            return gameLog;
        }
    }
}