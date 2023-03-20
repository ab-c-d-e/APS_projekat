using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;

namespace TheScientistAPI.Service
{
    public class GenericRepository<T>:IGenericRepository<T> where T:class
    {

        protected ScientistContext _context;
        protected ILogger _logger;
        protected DbSet<T> _dbSet;

        public GenericRepository(ScientistContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }
    }
}
