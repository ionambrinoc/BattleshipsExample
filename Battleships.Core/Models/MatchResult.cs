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

        public void CopyFrom(MatchResult otherMatchResult)
        {
            Winner = otherMatchResult.Winner;
            Loser = otherMatchResult.Loser;
            WinnerWins = otherMatchResult.WinnerWins;
            LoserWins = otherMatchResult.LoserWins;
            TimePlayed = otherMatchResult.TimePlayed;
        }

        public bool SamePlayers(MatchResult otherMatchResult)
        {
            if (Winner.Id != otherMatchResult.Winner.Id && Winner.Id != otherMatchResult.Loser.Id)
            {
                return false;
            }
            return Loser.Id == otherMatchResult.Winner.Id || Loser.Id == otherMatchResult.Loser.Id;
        }
    }
}
