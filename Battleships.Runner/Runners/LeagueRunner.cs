namespace Battleships.Runner.Runners
{
    using Battleships.Core.Models;
    using Battleships.Player;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeagueRunner
    {
        List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, IEnumerable<IBattleshipsPlayer> updatedPlayers, int numberOfRounds = 100);
    }

    public class LeagueRunner : ILeagueRunner
    {
        private readonly IMatchRunner matchRunner;

        public LeagueRunner(IMatchRunner matchRunner)
        {
            this.matchRunner = matchRunner;
        }

        public List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, IEnumerable<IBattleshipsPlayer> updatedPlayers, int numberOfRounds = 100)
        {
            var results = new List<MatchResult>();
            var playersCopy = new List<IBattleshipsPlayer>(players);
            foreach (var recentlyUpdatedPlayer in updatedPlayers)
            {
                playersCopy.Remove(recentlyUpdatedPlayer);
                results.AddRange(playersCopy.Select(player => matchRunner.GetMatchResult(recentlyUpdatedPlayer, player, numberOfRounds)));
            }
            return results;
        }
    }
}