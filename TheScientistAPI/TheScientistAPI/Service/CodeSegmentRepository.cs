using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class CodeSegmentRepository : GenericRepository<CodeSegment>, ICodeSegmentRepository
    {
        public CodeSegmentRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
