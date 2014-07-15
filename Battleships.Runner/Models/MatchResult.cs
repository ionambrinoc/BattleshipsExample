namespace Battleships.Runner.Models
{
    using System;

    public class MatchResult
    {
        public int Id { get; set; }
        public PlayerRecord Winner { get; set; }
        public PlayerRecord Loser { get; set; }
        public int WinnerWins { get; set; }
        public int LoserWins { get; set; }
        public DateTime TimePlayed { get; set; }
    }
}