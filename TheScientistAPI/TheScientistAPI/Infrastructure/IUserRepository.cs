using TheScientistAPI.Model;

namespace TheScientistAPI.Infrastructure
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<bool> Delete(string id);
        Task<User> GetById(string id);
    }
}
