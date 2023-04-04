using Microsoft.EntityFrameworkCore;
using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class ReferenceRepository : GenericRepository<Reference>, IReferenceRepository
    {
        public ReferenceRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }

        public Reference? GetByIdWithAuthors(int id)
        {
            var query = _context.Set<Reference>().AsQueryable();
            query = query.Include(sp => sp.Authors);
            return query.SingleOrDefault(sp => sp.Id == id);
        }
    }
}
