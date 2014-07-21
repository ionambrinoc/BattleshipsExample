namespace Battleships.Runner
{
    using Battleships.Runner.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeagueResults
    {
        List<KeyValuePair<PlayerRecord, int>> GenerateLeaderboard();
    }

    public class LeagueResults : ILeagueResults
    {
        private readonly List<MatchResult> results;

        public LeagueResults(List<MatchResult> results)
        {
            this.results = results;
        }

        public List<KeyValuePair<PlayerRecord, int>> GenerateLeaderboard()
        {
            var playerWins = GetPlayerWins();
            return playerWins.OrderBy(x => x.Value).ToList();
        }

        private Dictionary<PlayerRecord, int> GetPlayerWins()
        {
            var playerWins = new Dictionary<PlayerRecord, int>();
            foreach (var result in results)
            {
                if (playerWins.ContainsKey(result.Winner))
                {
                    playerWins[result.Winner]++;
                }
                else
                {
                    playerWins.Add(result.Winner, 1);
                }
                if (!playerWins.ContainsKey(result.Loser))
                {
                    playerWins.Add(result.Loser, 0);
                }
            }
            return playerWins;
        }
    }
}