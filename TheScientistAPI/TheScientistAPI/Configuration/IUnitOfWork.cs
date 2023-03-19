using TheScientistAPI.Infrastructure;

namespace TheScientistAPI.Configuration
{
    public interface IUnitOfWork
    {
        IScientificPaperRepository ScientificPapers { get; }
        ISectionRepository Sections { get; }
        IUserRolesRepository UserRoles { get; }
        Task CompleteAsync();
    }
}
