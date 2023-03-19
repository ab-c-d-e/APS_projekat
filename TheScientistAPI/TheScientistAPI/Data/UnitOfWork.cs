using Microsoft.AspNetCore.Identity;
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
        public IScientificPaperRepository ScientificPapers { get; set; }

        public ISectionRepository Sections { get; set; }

        public UnitOfWork(ScientistContext context, ILoggerFactory logger)
        {
            _context=context;
            _logger = logger.CreateLogger("logs");

            ScientificPapers = new ScientificPaperRepository(_context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
