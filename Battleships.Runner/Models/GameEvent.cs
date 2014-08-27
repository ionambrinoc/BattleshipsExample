namespace Battleships.Runner.Models
{
    using Battleships.Player.Interface;
    using System;

    public class GameEvent
    {
        public DateTime Time { get; set; }
        public bool IsPlayer1Turn { get; set; }
        public IGridSquare SelectedTarget { get; set; }
        public bool IsHit { get; set; }
    }
}