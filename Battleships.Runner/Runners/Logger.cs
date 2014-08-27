namespace Battleships.Runner.Runners
{
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Runner.Models;
    using JsonPrettyPrinterPlus;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Script.Serialization;

    public interface ILogger
    {
        void CreateTextFile();
        void NewGameLog();
        void AddGameEvent(bool isPlayerOneTurn, IGridSquare target, bool defendingIsHit);
        void AddGameResult(GameResult gameResult);
    }

    public class Logger : ILogger
    {
        public Logger(IBattleshipsPlayer playerOne, IBattleshipsPlayer playerTwo) //Constructor
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            GameLogs = new List<GameLog>();
            LoggerTextFileName = String.Format("{0}_VS_{1}", PlayerOne.Name, PlayerTwo.Name);
        }

        private List<GameLog> GameLogs { get; set; }
        private string LoggerTextFileName { get; set; }
        private IBattleshipsPlayer PlayerOne { get; set; }
        private IBattleshipsPlayer PlayerTwo { get; set; }

        public void NewGameLog()
        {
            var detailedLog = new List<GameEvent>();
            var gameLog = new GameLog(GameLogs.Count(), PlayerOne.GetShipPositions(), PlayerTwo.GetShipPositions(), detailedLog);
            GameLogs.Add(gameLog);
        }

        public void AddGameEvent(bool isPlayerOneTurn, IGridSquare target, bool defendingIsHit)
        {
            GameLogs[GameLogs.Count - 1].AddGameEvent(isPlayerOneTurn, target, defendingIsHit);
        }

        public void AddGameResult(GameResult gameResult)
        {
            GameLogs[GameLogs.Count - 1].AddResults(gameResult, PlayerOne);
        }

        public void CreateTextFile()
        {
            foreach (var gameLog in GameLogs)
            {
                var json = new JavaScriptSerializer().Serialize(gameLog).PrettyPrintJson();
                File.WriteAllText(@HostingEnvironment.ApplicationPhysicalPath + "\\GameLogs" + @String.Format("\\{0}_Game_{1}.txt", LoggerTextFileName, gameLog.GameNumber), json);
            }
        }
    }
}