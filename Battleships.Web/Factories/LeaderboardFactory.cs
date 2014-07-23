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
                AddWin(statsByPlayer, result.Winner, result.WinnerWins);
                AddLoss(statsByPlayer, result.Loser, result.LoserWins);
                AddRoundStats(statsByPlayer, result);
            }
            return statsByPlayer.Select(x => x.Value).ToList();
        }

        private static void AddRoundStats(Dictionary<PlayerRecord, PlayerStats> statsByPlayer, MatchResult result)
        {
            var winnerRoundStats = new RoundStats
                                   {
                                       OpponentName = result.Loser.Name,
                                       Wins = result.WinnerWins,
                                       Losses = result.LoserWins
                                   };
            var loserRoundStats = new RoundStats
                                  {
                                      OpponentName = result.Winner.Name,
                                      Wins = result.LoserWins,
                                      Losses = result.WinnerWins
                                  };
            statsByPlayer[result.Winner].RoundStats.Add(winnerRoundStats);
            statsByPlayer[result.Loser].RoundStats.Add(loserRoundStats);
        }

        private static void AddLoss(IDictionary<PlayerRecord, PlayerStats> statsByPlayer, PlayerRecord loser, int roundWins)
        {
            if (statsByPlayer.ContainsKey(loser))
            {
                statsByPlayer[loser].Losses++;
                statsByPlayer[loser].RoundWins += roundWins;
            }
            else
            {
                statsByPlayer.Add(loser, new PlayerStats
                                         {
                                             Id = loser.Id,
                                             Name = loser.Name,
                                             Losses = 1,
                                             RoundWins = roundWins,
                                             RoundStats = new List<RoundStats>()
                                         });
            }
        }

        private static void AddWin(IDictionary<PlayerRecord, PlayerStats> statsByPlayer, PlayerRecord winner, int roundWins)
        {
            if (statsByPlayer.ContainsKey(winner))
            {
                statsByPlayer[winner].Wins++;
                statsByPlayer[winner].RoundWins += roundWins;
            }
            else
            {
                statsByPlayer.Add(winner, new PlayerStats
                                          {
                                              Id = winner.Id,
                                              Name = winner.Name,
                                              Wins = 1,
                                              RoundWins = roundWins,
                                              RoundStats = new List<RoundStats>()
                                          });
            }
        }
    }
}