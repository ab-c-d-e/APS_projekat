using Microsoft.EntityFrameworkCore;
using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class ToDoListRepository : GenericRepository<ToDoList>, IToDoListRepository
    {
        public ToDoListRepository(ScientistContext context, ILogger logger) : base(context, logger) { }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var existing = await _dbSet.Where(u => u.ID == id)
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    _dbSet.Remove(existing);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo}, Delete method error", typeof(UserRepository));
                return false;
            }
        }
    }
}
