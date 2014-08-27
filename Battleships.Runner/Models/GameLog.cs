namespace Battleships.Runner.Models
{
    using Battleships.Player;
    using Battleships.Player.Interface;
    using System;
    using System.Collections.Generic;

    public class GameLog
    {
        public GameLog(int gameNumber, IEnumerable<IShipPosition> playerOneShipPositions, IEnumerable<IShipPosition> playerTwoShipPositions, ICollection<GameEvent> detailedLog)
        {
            GameNumber = gameNumber;
            Player1Positions = playerOneShipPositions;
            Player2Positions = playerTwoShipPositions;
            DetailedLog = detailedLog;
        }

        public bool Player1Won { get; set; }
        public ResultType ResultType { get; set; }
        public ICollection<GameEvent> DetailedLog { get; set; }
        public int GameNumber { get; set; }
        private IEnumerable<IShipPosition> Player1Positions { get; set; }
        private IEnumerable<IShipPosition> Player2Positions { get; set; }

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

        public void AddResults(GameResult gameResult, IBattleshipsPlayer playerOne)
        {
            ResultType = gameResult.ResultType;
            Player1Won = gameResult.Winner == playerOne;
        }
    }
}