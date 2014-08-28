namespace Battleships.Runner.Runners
{
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Runner.Models;
    using JsonPrettyPrinterPlus;
    using System;
    using System.IO;
    using System.Web.Script.Serialization;

    public interface ILogger
    {
        void StartGame();
        void AddGameEvent(bool isPlayerOneTurn, IGridSquare target, bool defendingIsHit);
        void CompleteGame(GameResult gameResult);
    }

    public class Logger : ILogger
    {
        private int currentLogNumber;

        public Logger(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            LogFilePrefix = String.Format("{0}_VS_{1}", PlayerOne.Name, PlayerTwo.Name);
        }

        public GameLog CurrentLog { get; set; }
        private string LogFilePrefix { get; set; }
        private IBattleshipsPlayer PlayerOne { get; set; }
        private IBattleshipsPlayer PlayerTwo { get; set; }

        public void StartGame()
        {
            CurrentLog = new GameLog(++currentLogNumber, PlayerOne, PlayerTwo);
        }

        public void AddGameEvent(bool isPlayerOneTurn, IGridSquare target, bool defendingIsHit)
        {
            CurrentLog.AddGameEvent(isPlayerOneTurn, target, defendingIsHit);
        }

        public void CompleteGame(GameResult gameResult)
        {
            CurrentLog.AddResults(gameResult);

            var json = new JavaScriptSerializer().Serialize(CurrentLog).PrettyPrintJson();

            File.WriteAllText(CurrentLog.GetPath(LogFilePrefix, currentLogNumber), json);
        }
    }
}