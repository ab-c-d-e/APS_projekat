using TheScientistAPI.Data;
using TheScientistAPI.Infrastructure;

namespace TheScientistAPI.Service
{
    public class UserRepo:IUser
    {
        private ScientistContext _context;

        public UserRepo(ScientistContext context)
        {
            _context = context;
        }   
    }
}
