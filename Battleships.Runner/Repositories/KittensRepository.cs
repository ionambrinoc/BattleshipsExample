namespace Battleships.Runner.Repositories
{
    using Battleships.Runner.Models;

    public interface IKittensRepository : IRepository<Kitten> {}

    public class KittensRepository : Repository<Kitten>, IKittensRepository
    {
        public KittensRepository(BattleshipsContext context) : base(context) {}
    }
}