namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;
    using System;
    using System.Collections.Generic;

    public interface IPastGameRepository : IRepository<PastGame> {}

    public class PastGameRepository : Repository<PastGame>, IPastGameRepository
    {
        public PastGameRepository(BattleshipsContext context)
            : base(context)
        {
            //PastGame objects are currently hard-coded. A client would use the add method of list to add records to the repository.
            var pastgame1 = new PastGame
                            {
                                FirstPlayer = "Jasper",
                                SecondPlayer = "Bingqian",
                                FirstPlayerWon = true,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };

            var pastgame2 = new PastGame
                            {
                                FirstPlayer = "Jasper",
                                SecondPlayer = "Bingqian",
                                FirstPlayerWon = false,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };

            var pastgame3 = new PastGame
                            {
                                FirstPlayer = "Jasper",
                                SecondPlayer = "Bingqian",
                                FirstPlayerWon = false,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };

            var pastgame4 = new PastGame
                            {
                                FirstPlayer = "Jasper",
                                SecondPlayer = "Bingqian",
                                FirstPlayerWon = false,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };

            PastGames = new List<PastGame> { pastgame1, pastgame2, pastgame3, pastgame4 };
        }

        public IEnumerable<PastGame> PastGames { get; set; }

        public new IEnumerable<PastGame> GetAll()
        {
            return PastGames;
        }
    }
}