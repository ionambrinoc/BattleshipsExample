namespace Battleships.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class GameLog
    {
        public int Id { get; set; }
        public virtual PlayerRecord Player1 { get; set; }
        public virtual PlayerRecord Player2 { get; set; }
        public DateTime StartTime { get; set; }
        public bool Player1Won { get; set; }
        public ResultType ResultType { get; set; }

        public string Player1PositionsString { get; set; }
        public string Player2PositionsString { get; set; }
        public virtual ICollection<GameEvent> DetailedLog { get; set; }
    }
}