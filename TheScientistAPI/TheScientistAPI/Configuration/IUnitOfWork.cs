using TheScientistAPI.Infrastructure;

namespace TheScientistAPI.Configuration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IScientificPaperRepository ScientificPapers { get; }
        ISectionRepository Sections { get; }
        IImageRepository Images { get; }
        ITextSegmentRepository TextSegments { get; }
        ICodeSegmentRepository CodeSegments { get; }
        IToDoListRepository ToDoLists { get; }
        IToDoItemRepository TodoItems { get; }
        INotificationRepository Notifications { get; }
        IMessageRepository Messages { get; }

        Task CompleteAsync();
    }
}
