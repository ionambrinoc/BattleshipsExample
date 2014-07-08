namespace Battleships.Runner.Models
{
    using System;

    public class PastGame
    {
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public bool FirstPlayerWon { get; set; }
        public DateTime TimePlayed { get; set; }
        public string Winner {
            get
            {
                if (FirstPlayerWon)
                {
                    return FirstPlayer;
                }
                else
                {
                    return SecondPlayer;
                }
            }
            set
            {
                if (value == FirstPlayer)
                {
                    FirstPlayerWon = true;
                }
                else
                {
                    FirstPlayerWon = false;
                }
            }
        }
    }
}
