using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class SectionRepository : GenericRepository<Section>, ISectionRepository
    {
        public SectionRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {}
        public Section? GetById(int id, bool includeSubsections)
        {
            var query = _context.Set<Section>().AsQueryable();

            if (includeSubsections)
            {
                query = query.Include(sp => sp.Subsections);
            }
            query.Include(sp => sp.Paper);
            return query.SingleOrDefault(sp => sp.Id == id);
        }
    }
}
