namespace Battleships.Core.Models
{
    using Battleships.Player.Interface;
    using System;

    public class GameEvent
    {

        public GameEvent(DateTime time, bool isPlayer1Turn, IGridSquare selectedTarget, bool isHit)
        {
            Time = time;
            IsPlayer1Turn = isPlayer1Turn;
            SelectedTarget = selectedTarget;
            IsHit = isHit;
        }
        public DateTime Time { get; private set; }
        public bool IsPlayer1Turn { get; private set; }
        public IGridSquare SelectedTarget { get; private set; }
        public bool IsHit { get; private set; }
    }
}