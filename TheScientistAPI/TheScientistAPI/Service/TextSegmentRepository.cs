using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class TextSegmentRepository : GenericRepository<Image>, ITextSegmentRepository
    {
        public TextSegmentRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
