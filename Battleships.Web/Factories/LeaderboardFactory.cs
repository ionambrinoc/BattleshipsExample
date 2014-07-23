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
            }
            return statsByPlayer.Select(x => x.Value).ToList();
        }

        private static void AddLoss(IDictionary<PlayerRecord, PlayerStats> playerWins, PlayerRecord loser, int roundWins)
        {
            if (playerWins.ContainsKey(loser))
            {
                playerWins[loser].Losses++;
                playerWins[loser].RoundWins += roundWins;
            }
            else
            {
                playerWins.Add(loser, new PlayerStats
                                      {
                                          Id = loser.Id,
                                          Name = loser.Name,
                                          Losses = 1,
                                          RoundWins = roundWins
                                      });
            }
        }

        private static void AddWin(IDictionary<PlayerRecord, PlayerStats> playerWins, PlayerRecord winner, int roundWins)
        {
            if (playerWins.ContainsKey(winner))
            {
                playerWins[winner].Wins++;
                playerWins[winner].RoundWins += roundWins;
            }
            else
            {
                playerWins.Add(winner, new PlayerStats
                                       {
                                           Id = winner.Id,
                                           Name = winner.Name,
                                           Wins = 1,
                                           RoundWins = roundWins
                                       });
            }
        }
    }
}