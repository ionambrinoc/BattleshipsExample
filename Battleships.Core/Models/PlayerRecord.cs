namespace Battleships.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayerRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }
        public string PictureFileName { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ICollection<MatchResult> WonMatchResults { get; set; }
        public virtual ICollection<MatchResult> LostMatchResults { get; set; }

        public void MarkAsUpdated()
        {
            LastUpdated = DateTime.Now;
        }

        public virtual DateTime GetLastTimePlayed()
        {
            var timeOfLastWonMatch = WonMatchResults.Count != 0 ? WonMatchResults.Max(result => result.TimePlayed) : DateTime.MinValue;
            var timeOfLastLostMatch = LostMatchResults.Count != 0 ? LostMatchResults.Max(result => result.TimePlayed) : DateTime.MinValue;

            return timeOfLastWonMatch > timeOfLastLostMatch ? timeOfLastWonMatch : timeOfLastLostMatch;
        }
    }
}