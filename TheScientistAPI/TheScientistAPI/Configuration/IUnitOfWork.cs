using TheScientistAPI.Infrastructure;

namespace TheScientistAPI.Configuration
{
    public interface IUnitOfWork
    {
        IScientificPaperRepository ScientificPapers { get; }
        ISectionRepository Sections { get; }
        IReferenceRepository References { get; }
        IUserRolesRepository UserRoles { get; }
        IKeywordRepository Keywords { get; }
        IMessageUserRepository MessageUsers { get; }
        Task CompleteAsync();
    }
}
