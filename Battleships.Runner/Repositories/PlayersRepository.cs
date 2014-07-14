namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public interface IPlayersRepository : IRepository<Player>
    {
        Player GetPlayerById(int id);
    };

    [ExcludeFromCodeCoverage]
    public class PlayersRepository : Repository<Player>, IPlayersRepository
    {
        public PlayersRepository(BattleshipsContext context) : base(context) {}

        public Player GetPlayerById(int id)
        {
            return Entities.AsQueryable().First(x => x.Id == id);
        }
    }
}
