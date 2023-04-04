using TheScientistAPI.Model;

namespace TheScientistAPI.Infrastructure
{
    public interface IMessageUserRepository:IGenericRepository<MessageUser>
    {
        List<MessageUser> GetByUserAndPaper(string userId, int paperId);
    }
}
