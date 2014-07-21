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
            return playerWins.OrderByDescending(x => x.Value).ToList();
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
                    playerWins.Add(result.Winner, new PlayerStats(1, result.WinnerWins));
                }

                if (playerWins.ContainsKey(result.Loser))
                {
                    playerWins[result.Loser].Losses++;
                    playerWins[result.Loser].RoundWins += result.LoserWins;
                }
                else
                {
                    playerWins.Add(result.Loser, new PlayerStats(roundWins: result.LoserWins, losses: 1));
                }
            }
            return playerWins;
        }
    }
}