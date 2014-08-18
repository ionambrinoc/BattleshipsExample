namespace Battleships.Core.Models
{
    using Battleships.Player.Interface;
    using Battleships.Runner.Models;
    using System;
    using System.Collections.Generic;

    public interface IGameLog
    {
        PlayerRecord Player1 { get; }
        PlayerRecord Player2 { get; }
        DateTime StartTime { get; }
        bool Player1Won { get; }
        ResultType ResultType { get; }

        IShipPosition[] Player1Positions { get; }
        IShipPosition[] Player2Positions { get; }
        List<GameEvent> DetailedLog { get; }
    }

    public class GameLog : IGameLog
    {
        public GameLog(PlayerRecord player1, PlayerRecord player2, DateTime startTime, IShipPosition[] player1Positions, IShipPosition[] player2Positions)
        {
            Player1 = player1;
            Player2 = player2;
            StartTime = startTime;
            Player1Positions = player1Positions;
            Player2Positions = player2Positions;
            DetailedLog = new List<GameEvent>();
        }

        public PlayerRecord Player1 { get; private set; }
        public PlayerRecord Player2 { get; private set; }
        public DateTime StartTime { get; private set; }
        public bool Player1Won { get; private set; }
        public ResultType ResultType { get; private set; }

        public IShipPosition[] Player1Positions { get; private set; }
        public IShipPosition[] Player2Positions { get; private set; }
        public List<GameEvent> DetailedLog { get; private set; }

        public void SetGameResult(bool player1Won, ResultType resultType)
        {
            Player1Won = player1Won;
            ResultType = resultType;
        }

        public void AddGameEvent(DateTime time, bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit)
        {
            DetailedLog.Add(new GameEvent(time, isPlayer1Turn, selectedTarget, isHit));
        }
    }
}