namespace Battleships.Runner.Repositories
{
    using System.Data.Entity;

    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void SaveContext();
    }

    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly BattleshipsContext context;

        protected Repository(BattleshipsContext context)
        {
            this.context = context;
        }

        protected DbSet<T> Entities
        {
            get { return context.Set<T>(); }
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
        }

        public void SaveContext()
        {
            context.SaveChanges();
        }
    }
}
