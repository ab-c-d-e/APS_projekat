using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class ToDoItemRepository : GenericRepository<ToDoItem>, IToDoItemRepository
    {
        public ToDoItemRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
