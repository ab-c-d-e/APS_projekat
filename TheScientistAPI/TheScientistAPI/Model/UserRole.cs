using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheScientistAPI.Model
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public ScientificPaper ScientificPaper { get; set; }
        public UserRoleType RoleType { get; set; }
    }

    public enum UserRoleType
    {
        Reviewer,
        Editor
    }
}
