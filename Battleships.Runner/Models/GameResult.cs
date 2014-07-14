namespace Battleships.Runner.Models
{
    using System;

    public class GameResult
    {
        public int Id { get; set; }
        public virtual Player Winner { get; set; }
        public virtual Player Loser { get; set; }
        public DateTime TimePlayed { get; set; }
    }
}