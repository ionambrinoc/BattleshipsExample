namespace Battleships.Runner.Runners
{
    using Battleships.Core.Models;
    using Battleships.Player;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeagueRunner
    {
        IEnumerable<MatchResult> GetLeagueResults(IEnumerable<IBattleshipsPlayer> players, IEnumerable<IBattleshipsPlayer> updatedPlayers, int numberOfRounds = 101);
    }

    public class LeagueRunner : ILeagueRunner
    {
        private readonly IMatchRunner matchRunner;

        public LeagueRunner(IMatchRunner matchRunner)
        {
            this.matchRunner = matchRunner;
        }

        public IEnumerable<MatchResult> GetLeagueResults(IEnumerable<IBattleshipsPlayer> players, IEnumerable<IBattleshipsPlayer> updatedPlayers, int numberOfRounds = 101)
        {
            var results = new List<MatchResult>();
            var playersToBePlayedAgainst = new HashSet<IBattleshipsPlayer>(players);
            foreach (var updatedPlayer in updatedPlayers)
            {
                playersToBePlayedAgainst.Remove(updatedPlayer);
                results.AddRange(playersToBePlayedAgainst.Select(player => matchRunner.GetMatchResult(updatedPlayer, player, numberOfRounds)));
            }
            return results;
        }
    }
}