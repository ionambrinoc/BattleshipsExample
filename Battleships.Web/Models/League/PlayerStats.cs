namespace Battleships.Web.Models.League
{
    using System;

    public class PlayerStats : IComparable<PlayerStats>
    {
        public int Id { get; set; }
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