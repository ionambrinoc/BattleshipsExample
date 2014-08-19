namespace Battleships.Core.Models
{
    using System;

    public class GameEvent
    {
        public DateTime Time { get; set; }
        public bool IsPlayer1Turn { get; set; }
        public string SelectedTarget { get; set; }
        public bool IsHit { get; set; }
    }
}