namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using System;

    public class PlayerStats : IComparable<PlayerStats>
    {
        public PlayerStats(PlayerRecord player, int wins = 0, int roundWins = 0, int losses = 0)
        {
            Name = player.Name;
            Wins = wins;
            RoundWins = roundWins;
            Losses = losses;
        }

        public string Name { get; set; }
        public int Wins { get; set; }
        public int RoundWins { get; set; }
        public int Losses { get; set; }

        public int CompareTo(PlayerStats other)
        {
            return Wins == other.Wins ? RoundWins.CompareTo(other.RoundWins) : Wins.CompareTo(other.Wins);
        }
    }
}