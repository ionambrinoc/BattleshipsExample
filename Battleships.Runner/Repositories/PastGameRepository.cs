namespace Battleships.Runner.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using Battleships.Runner.Models;

    public interface IPastGameRepository : IRepository<PastGame> {}

    public class PastGameRepository : Repository<PastGame>, IPastGameRepository
    {

        private IEnumerable<PastGame> pastGames;




        public PastGameRepository(BattleshipsContext context) : base(context)
        {
            //PastGame objects are currently hard-coded. A client would use the add method of list to add records to the repository.
            var pastgame1 = new PastGame
                            {
                                FirstPlayer = "Jasper",
                                SecondPlayer = "Bingqian",
                                FirstPlayerWon = true,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };



            pastGames = new List<PastGame> { pastgame1 };
        }

        public new IEnumerable<PastGame> GetAll()
        {
            return pastGames;
        }
    }
}
