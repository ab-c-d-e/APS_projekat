using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class AddUserDto
    {
        public int PaperId { get; set; }
        public string UserName { get; set; }
        public UserRoleType Role { get; set; }
    }
}
