using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        public ImageRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
