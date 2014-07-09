namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;
    using System;
    using System.Collections.Generic;

    public interface IGameResultsRepository : IRepository<GameResult> {}

    public class GameResultsRepository : Repository<GameResult>, IGameResultsRepository
    {
        public GameResultsRepository(BattleshipsContext context)
            : base(context)
        {
            // GameResult objects are currently hard-coded. A client would use the add method of list to add records to the repository.
            var player1 = new Player();
            var player2 = new Player();
            player1.Name = "Jasper";
            player2.Name = "Bingqian";

            var pastgame1 = new GameResult
                            {
                                Winner = player1,
                                Loser = player2,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };
            var pastgame2 = new GameResult
                            {
                                Winner = player1,
                                Loser = player2,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };

            var pastgame3 = new GameResult
                            {
                                Winner = player1,
                                Loser = player2,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };

            var pastgame4 = new GameResult
                            {
                                Winner = player1,
                                Loser = player2,
                                TimePlayed = new DateTime(2014, 7, 7, 12, 12, 12),
                            };

            PastGames = new List<GameResult> { pastgame1, pastgame2, pastgame3, pastgame4 };
        }

        public IEnumerable<GameResult> PastGames { get; set; }

        public new IEnumerable<GameResult> GetAll()
        {
            return PastGames;
        }
    }
}