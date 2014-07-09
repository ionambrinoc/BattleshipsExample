namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;

    public interface IPlayersRepository : IRepository<Player> {};

    public class PlayersRepository : Repository<Player>, IPlayersRepository
    {
        public PlayersRepository(BattleshipsContext context) : base(context) {}
    }
}