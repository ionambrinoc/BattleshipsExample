namespace Battleships.Core.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void SaveContext();
    }

    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;

        protected Repository(DbContext context)
        {
            this.context = context;
        }

        protected DbSet<T> Entities
        {
            get { return context.Set<T>(); }
        }

        public IEnumerable<T> GetAll()
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

        public void RemoveRange(IEnumerable<T> range)
        {
            Entities.RemoveRange(range);
        }
    }
}