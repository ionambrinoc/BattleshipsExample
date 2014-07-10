namespace Battleships.Runner.Models
{
    using System;

    public class PastGame
    {
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public bool FirstPlayerWon { get; set; }
        public DateTime TimePlayed { get; set; }

        public string Winner
        {
            get { return FirstPlayerWon ? FirstPlayer : SecondPlayer; }
            set { FirstPlayerWon = value == FirstPlayer; }
        }
    }
}
