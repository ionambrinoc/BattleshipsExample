namespace Battleships.Core.Models
{
    using System;

    public class MatchResult
    {
        public int Id { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
        public virtual PlayerRecord Winner { get; set; }
        public virtual PlayerRecord Loser { get; set; }
        public int WinnerWins { get; set; }
        public int LoserWins { get; set; }
        public DateTime TimePlayed { get; set; }
    }
}
