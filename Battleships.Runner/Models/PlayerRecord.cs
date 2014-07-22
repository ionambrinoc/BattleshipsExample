namespace Battleships.Runner.Models
{
    using System.Collections.Generic;

    public class PlayerRecord
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PictureFileName { get; set; }

        public virtual ICollection<MatchResult> WonMatchResults { get; set; }
        public virtual ICollection<MatchResult> LostMatchResults { get; set; }
    }
}