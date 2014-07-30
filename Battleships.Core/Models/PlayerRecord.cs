namespace Battleships.Core.Models
{
    using System.Collections.Generic;

    public class PlayerRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }
        public string PictureFileName { get; set; }

        public virtual ICollection<MatchResult> WonMatchResults { get; set; }
        public virtual ICollection<MatchResult> LostMatchResults { get; set; }
    }
}
