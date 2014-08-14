namespace Battleships.Runner.Runners
{
    using Battleships.Core.Models;
    using Battleships.Player;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeagueRunner
    {
        List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, List<IBattleshipsPlayer> recentlyUpdatedPlayers, int numberOfRounds = 100);
    }

    public class LeagueRunner : ILeagueRunner
    {
        private readonly IMatchRunner matchRunner;

        public LeagueRunner(IMatchRunner matchRunner)
        {
            this.matchRunner = matchRunner;
        }

        public List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, List<IBattleshipsPlayer> recentlyUpdatedPlayers, int numberOfRounds = 100)
        {
            var results = new List<MatchResult>();
            foreach (var recentlyUpdatedPlayer in recentlyUpdatedPlayers)
            {
                players.Remove(recentlyUpdatedPlayer);
                results.AddRange(players.Select(player => matchRunner.GetMatchResult(recentlyUpdatedPlayer, player, numberOfRounds)));
            }
            return results;
        }
    }
}