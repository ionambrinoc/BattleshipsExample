namespace Battleships.Runner
{
    using Battleships.Player;
    using Battleships.Runner.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILeagueRunner
    {
        List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, int numberOfRounds = 1);
    }

    public class LeagueRunner : ILeagueRunner
    {
        private readonly IMatchRunner matchRunner;

        public LeagueRunner(IMatchRunner matchRunner)
        {
            this.matchRunner = matchRunner;
        }

        public List<MatchResult> GetLeagueResults(List<IBattleshipsPlayer> players, int numberOfRounds = 1)
        {
            return (from playerOne in players from playerTwo in players select matchRunner.GetMatchResult(playerOne, playerTwo, numberOfRounds)).ToList();
        }
    }
}