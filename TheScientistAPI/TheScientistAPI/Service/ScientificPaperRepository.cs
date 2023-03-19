using Microsoft.EntityFrameworkCore;
using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class ScientificPaperRepository:GenericRepository<ScientificPaper>, IScientificPaperRepository
    {
        public ScientificPaperRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }

        public ScientificPaper GetById(int id, bool includeRelatedEntities)
        {
            var query = _context.Set<ScientificPaper>().AsQueryable();

            if (includeRelatedEntities)
            {
                query = query.Include(sp => sp.Keywords);
            }

            return query.SingleOrDefault(sp => sp.Id == id);
        }
    }
}
