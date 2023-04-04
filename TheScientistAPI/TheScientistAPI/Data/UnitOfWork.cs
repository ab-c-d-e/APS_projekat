using TheScientistAPI.Configuration;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Service;

namespace TheScientistAPI.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ScientistContext _context;
        private readonly ILogger _logger;
        public IScientificPaperRepository ScientificPapers { get; set; }
        public ISectionRepository Sections { get; set; }
        public IUserRolesRepository UserRoles { get; set; }

        public IKeywordRepository Keywords { get; set; }

        public IReferenceRepository References { get; set; }

        public IMessageUserRepository MessageUsers { get; set; }

        public UnitOfWork(ScientistContext context, ILoggerFactory logger)
        {
            _context=context;
            _logger = logger.CreateLogger("logs");

            ScientificPapers = new ScientificPaperRepository(_context, _logger);
            Sections = new SectionRepository(_context, _logger);
            UserRoles = new UserRolesRepository(_context, _logger);
            Keywords = new KeywordRepository(_context, _logger);
            References= new ReferenceRepository(_context, _logger);
            MessageUsers = new MessageUserRepository(_context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
