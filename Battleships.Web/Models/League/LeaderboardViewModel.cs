namespace Battleships.Web.Models.League
{
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeaderboardViewModel
    {
        List<PlayerStats> GenerateLeaderboard(List<MatchResult> results);
    }

    public class LeaderboardViewModel : ILeaderboardViewModel
    {
        public List<PlayerStats> GenerateLeaderboard(List<MatchResult> results)
        {
            return GetPlayerWins(results).Select(x => x.Value).OrderByDescending(x => x).ToList();
        }

        private Dictionary<PlayerRecord, PlayerStats> GetPlayerWins(IEnumerable<MatchResult> results)
        {
            var playerWins = new Dictionary<PlayerRecord, PlayerStats>();
            foreach (var result in results)
            {
                if (playerWins.ContainsKey(result.Winner))
                {
                    playerWins[result.Winner].Wins++;
                    playerWins[result.Winner].RoundWins += result.WinnerWins;
                }
                else
                {
                    playerWins.Add(result.Winner, new PlayerStats(result.Winner, 1, result.WinnerWins));
                }

                if (playerWins.ContainsKey(result.Loser))
                {
                    playerWins[result.Loser].Losses++;
                    playerWins[result.Loser].RoundWins += result.LoserWins;
                }
                else
                {
                    playerWins.Add(result.Loser, new PlayerStats(result.Loser, roundWins: result.LoserWins, losses: 1));
                }
            }
            return playerWins;
        }
    }
}