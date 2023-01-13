using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class SectionRepository : GenericRepository<Section>, ISectionRepository
    {
        public SectionRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
