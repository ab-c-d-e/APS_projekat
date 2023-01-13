using Microsoft.EntityFrameworkCore;
using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class ScientificPaperRepository : GenericRepository<ScientificPaper>, IScientificPaperRepository
    {
        public ScientificPaperRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<bool> Upsert(ScientificPaper entity)
        {
            try
            {
                var existing = await _dbSet.Where(u => u.ID == entity.ID)
                    .FirstOrDefaultAsync();

                if (existing == null)
                    return await Add(entity);

                existing.Title = entity.Title;
                existing.Description = entity.Description;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo}, Upsert method error", typeof(ScientificPaperRepository));
                return false;
            }
        }

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
                _logger.LogError(ex, "{Repo}, Delete method error", typeof(ScientificPaperRepository));
                return false;
            }
        }
    }
}
