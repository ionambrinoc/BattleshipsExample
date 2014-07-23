namespace Battleships.Runner
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeagueRunner
    {
        List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, int numberOfRounds = 100);
    }

    public class LeagueRunner : ILeagueRunner
    {
        private readonly IMatchRunner matchRunner;

        public LeagueRunner(IMatchRunner matchRunner)
        {
            this.matchRunner = matchRunner;
        }

        public List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, int numberOfRounds = 100)
        {
            var results = new List<MatchResult>();
            for (var i = 0; i < players.Count(); i++)
            {
                for (var j = i + 1; j < players.Count(); j++)
                {
                    results.Add(matchRunner.GetMatchResult(players[i], players[j], numberOfRounds));
                }
            }
            return results;
        }
    }
}