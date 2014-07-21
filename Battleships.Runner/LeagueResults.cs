namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeagueResults
    {
        List<KeyValuePair<PlayerRecord, PlayerStats>> GenerateLeaderboard(List<MatchResult> results);
    }

    public class LeagueResults : ILeagueResults
    {
        public List<KeyValuePair<PlayerRecord, PlayerStats>> GenerateLeaderboard(List<MatchResult> results)
        {
            var playerWins = GetPlayerWins(results);
            return playerWins.OrderBy(x => x.Value.Wins).ToList();
        }

        private Dictionary<PlayerRecord, PlayerStats> GetPlayerWins(IEnumerable<MatchResult> results)
        {
            var playerWins = new Dictionary<PlayerRecord, PlayerStats>();
            foreach (var result in results)
            {
                if (playerWins.ContainsKey(result.Winner))
                {
                    playerWins[result.Winner].Wins++;
                }
                else
                {
                    playerWins.Add(result.Winner, new PlayerStats(1));
                }

                if (playerWins.ContainsKey(result.Loser))
                {
                    playerWins[result.Loser].Losses++;
                }
                else
                {
                    playerWins.Add(result.Loser, new PlayerStats(losses: 1));
                }
            }
            return playerWins;
        }
    }

    public class PlayerStats
    {
        public PlayerStats(int wins = 0, int losses = 0)
        {
            Wins = wins;
            Losses = losses;
        }

        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}