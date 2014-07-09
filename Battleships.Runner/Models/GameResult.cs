namespace Battleships.Runner.Models
{
    using System;

    public class GameResult
    {
        public int Id { get; set; }
        public Player Winner { get; set; }
        public Player Loser { get; set; }
        public DateTime TimePlayed { get; set; }
    }
}