namespace Battleships.Web.Factories
{
    using Battleships.Runner.Models;
    using Battleships.Web.Models.League;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeaderboardFactory
    {
        List<PlayerStats> GenerateLeaderboard(List<MatchResult> results);
    }

    public class LeaderboardFactory : ILeaderboardFactory
    {
        public List<PlayerStats> GenerateLeaderboard(List<MatchResult> results)
        {
            return GetAggregatedStats(results).OrderByDescending(x => x).ToList();
        }

        private static IEnumerable<PlayerStats> GetAggregatedStats(IEnumerable<MatchResult> results)
        {
            var statsByPlayer = new Dictionary<PlayerRecord, PlayerStats>();
            foreach (var result in results)
            {
                AddWin(statsByPlayer, result.Winner, result.Loser.Name, result.WinnerWins, result.LoserWins);
                AddLoss(statsByPlayer, result.Loser, result.Winner.Name, result.LoserWins, result.WinnerWins);
            }
            return statsByPlayer.Select(x => x.Value).ToList();
        }

        private static void AddWin(Dictionary<PlayerRecord, PlayerStats> statsByPlayer, PlayerRecord winner, string opponentName, int winnerRoundWins, int loserRoundWins)
        {
            if (statsByPlayer.ContainsKey(winner))
            {
                statsByPlayer[winner].Wins++;
            }
            else
            {
                statsByPlayer.Add(winner, new PlayerStats
                                          {
                                              Id = winner.Id,
                                              Name = winner.Name,
                                              Wins = 1
                                          });
            }
            AddRoundStats(statsByPlayer, winner, opponentName, winnerRoundWins, loserRoundWins);
        }

        private static void AddLoss(Dictionary<PlayerRecord, PlayerStats> statsByPlayer, PlayerRecord loser, string opponentName, int loserRoundWins, int winnerRoundWins)
        {
            if (statsByPlayer.ContainsKey(loser))
            {
                statsByPlayer[loser].Losses++;
            }
            else
            {
                statsByPlayer.Add(loser, new PlayerStats
                                         {
                                             Id = loser.Id,
                                             Name = loser.Name,
                                             Losses = 1
                                         });
            }
            AddRoundStats(statsByPlayer, loser, opponentName, loserRoundWins, winnerRoundWins);
        }

        private static void AddRoundStats(IDictionary<PlayerRecord, PlayerStats> statsByPlayer, PlayerRecord player, string opponentName, int playerRoundWins, int opponentRoundWins)
        {
            statsByPlayer[player].RoundStats.Add(new RoundStats
                                                 {
                                                     OpponentName = opponentName,
                                                     Wins = playerRoundWins,
                                                     Losses = opponentRoundWins
                                                 });
        }
    }
}
