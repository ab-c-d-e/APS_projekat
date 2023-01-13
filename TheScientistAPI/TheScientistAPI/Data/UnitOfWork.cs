using TheScientistAPI.Configuration;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;
using TheScientistAPI.Service;

namespace TheScientistAPI.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ScientistContext _context;
        private readonly ILogger _logger;
        public IUserRepository Users { get; set; }

        public IScientificPaperRepository ScientificPapers { get; set; }

        public ISectionRepository Sections { get; set; }

        public IImageRepository Images { get; set; }

        public ITextSegmentRepository TextSegments { get; set; }

        public ICodeSegmentRepository CodeSegments { get; set; }

        public IToDoListRepository ToDoLists { get; set; }

        public IToDoItemRepository TodoItems { get; set; }

        public INotificationRepository Notifications { get; set; }

        public IMessageRepository Messages { get; set; }

        public UnitOfWork(ScientistContext context, ILoggerFactory logger)
        {
            _context=context;
            _logger = logger.CreateLogger("logs");

            Users = new UserRepository(_context, _logger);
            ScientificPapers = new ScientificPaperRepository(_context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
