namespace Battleships.Runner.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
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

        public IQueryable<T> GetAll()
        {
            return Entities;
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