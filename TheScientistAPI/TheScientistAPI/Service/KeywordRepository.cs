using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class KeywordRepository : GenericRepository<Keyword>, IKeywordRepository
    {
        public KeywordRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }

        public Keyword? GetByName(string name)
        {
            var query = _context.Set<Keyword>().AsQueryable();
            return query.FirstOrDefault(keyword => keyword.Name == name);
        }
    }
}
