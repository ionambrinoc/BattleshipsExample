﻿namespace Battleships.Runner.Models
{
    using System;

    public class GameResult
    {
        public int Id { get; set; }
        public virtual PlayerRecord Winner { get; set; }
        public virtual PlayerRecord Loser { get; set; }
        public DateTime TimePlayed { get; set; }
    }
}