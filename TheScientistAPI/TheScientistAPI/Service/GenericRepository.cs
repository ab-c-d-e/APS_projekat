using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<List<T>> All()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetById(int id)
        {
            return 
                await _dbSet.FindAsync(id)?? 
                throw new Exception("Entity with this key does not exist");
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
