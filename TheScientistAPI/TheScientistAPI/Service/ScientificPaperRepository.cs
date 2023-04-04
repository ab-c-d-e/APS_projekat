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

        public ScientificPaper? GetById(int id, bool includeKeywords, bool includeUsers, bool includeSections, bool includeReferences, bool includeMessages)
        {
            var query = _context.Set<ScientificPaper>().AsQueryable();

            if (includeKeywords)
            {
                query = query.Include(sp => sp.Keywords);
            }

            if(includeUsers)
            {
                query = query.Include(sp => sp.UserRoles).ThenInclude(u=>u.User);
            }

            if(includeSections)
            {
                query = query.Include(sp => sp.Sections);
            }

            if(includeReferences)
            {
                query = query.Include(sp => sp.References)
                    .ThenInclude(r=>r.Authors);
            }

            if(includeMessages)
            {
                query = query.Include(sp => sp.Messages);
            }

            query = query.Include(sp => sp.Creator);

            return query.SingleOrDefault(sp => sp.Id == id);
        }
    }
}
