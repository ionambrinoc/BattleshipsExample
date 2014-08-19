namespace Battleships.Core.Factories
{
    using Battleships.Core.Models;
    using Battleships.Player.Interface;
    using Battleships.Runner.Models;
    using System;

    public interface IGameLogFactory
    {
        void InitialiseGameLog(PlayerRecord player1, PlayerRecord player2, DateTime startTime, IShipPosition[] player1Positions, IShipPosition[] player2Positions);
        void AddGameEvent(DateTime time, bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit);
        IGameLog GetCompleteGame(bool player1Won, ResultType resultType);
    }

    public class GameLogFactory : IGameLogFactory
    {
        private GameLog gameLog;

        public void InitialiseGameLog(PlayerRecord player1, PlayerRecord player2, DateTime startTime, IShipPosition[] player1Positions, IShipPosition[] player2Positions)
        {
            gameLog = new GameLog(player1, player2, startTime, player1Positions, player2Positions);
        }

        public void AddGameEvent(DateTime time, bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit)
        {
            gameLog.AddGameEvent(time, isPlayer1Turn, selectedTarget, isHit);
        }

        public IGameLog GetCompleteGame(bool player1Won, ResultType resultType)
        {
            gameLog.SetGameResult(player1Won, resultType);
            return gameLog;
        }
    }
}