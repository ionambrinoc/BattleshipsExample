namespace Battleships.Runner.Models
{
    using Battleships.Player;
    using Battleships.Player.Interface;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public interface IGameLog
    {
        void AddGameEvent(bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit);
        void AddResults(GameResult gameResult);
        string GetPath(string logFilePrefix, int logNumber);
    }

    public class GameLog : IGameLog
    {
        private readonly IBattleshipsPlayer playerOne;
        private readonly IBattleshipsPlayer playerTwo;

        public GameLog(int gameNumber, IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo, IEnumerable<IShipPosition> playerOneShipPositions, IEnumerable<IShipPosition> playerTwoShipPositions, ICollection<GameEvent> detailedLog)
        {
            GameNumber = gameNumber;
            this.playerOne = playerOne;
            this.playerTwo = playerTwo;
            Player1Positions = playerOneShipPositions;
            Player2Positions = playerTwoShipPositions;
            DetailedLog = detailedLog;
        }

        // Included in JSON serialization for log files
        // ReSharper disable once UnusedMember.Global
        public string PlayerOne
        {
            get { return playerOne.Name; }
        }

        // Included in JSON serialization for log files
        // ReSharper disable once UnusedMember.Global
        public string PlayerTwo
        {
            get { return playerTwo.Name; }
        }

        public int GameNumber { get; set; }
        public bool Player1Won { get; set; }
        public ResultType ResultType { get; set; }
        public IEnumerable<IShipPosition> Player1Positions { get; set; }
        public IEnumerable<IShipPosition> Player2Positions { get; set; }
        public ICollection<GameEvent> DetailedLog { get; set; }

        public void AddGameEvent(bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit)
        {
            var gameEvent = new GameEvent
                            {
                                Time = DateTime.Now,
                                IsHit = isHit,
                                IsPlayer1Turn = isPlayer1Turn,
                                SelectedTarget = selectedTarget,
                            };
            DetailedLog.Add(gameEvent);
        }

        public void AddResults(GameResult gameResult)
        {
            ResultType = gameResult.ResultType;
            Player1Won = gameResult.Winner == playerOne;
        }

        public string GetPath(string logFilePrefix, int logNumber)
        {
            var directory = DirectoryPath.GetFromAppSettings("GameLogsDirectory");
            var fileName = String.Concat(logFilePrefix, "_Game_", logNumber.ToString(CultureInfo.InvariantCulture), ".txt");
            return Path.Combine(directory, fileName);
        }
    }
}