namespace Battleships.Web.Models.League
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayerStats : IComparable<PlayerStats>
    {
        public PlayerStats()
        {
            RoundStats = new List<RoundStats>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }

        public int TotalRoundWins
        {
            get { return RoundStats.Sum(x => x.Wins); }
        }

        public int Losses { get; set; }
        public List<RoundStats> RoundStats { get; set; }

        public int CompareTo(PlayerStats other)
        {
            return Wins == other.Wins ? TotalRoundWins.CompareTo(other.TotalRoundWins) : Wins.CompareTo(other.Wins);
        }
    }
}