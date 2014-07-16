namespace Battleships.Runner.Models
{
    using System.Collections.Generic;

    public class PlayerRecord
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        public virtual ICollection<GameResult> WonGameResults { get; set; }
        public virtual ICollection<GameResult> LostGameResults { get; set; }
    }
}
