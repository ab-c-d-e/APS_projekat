using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;
using TheScientistAPI.Model;

namespace TheScientistAPI.Service
{
    public class UserRolesRepository:GenericRepository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(ScientistContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
