namespace Battleships.Runner.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public virtual ICollection<PlayerRecord> PlayerRecords { get; set; }
    }
}