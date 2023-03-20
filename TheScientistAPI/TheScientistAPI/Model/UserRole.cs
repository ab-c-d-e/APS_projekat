using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }

        [JsonIgnore]
        public ScientificPaper ScientificPaper { get; set; }
        public UserRoleType RoleType { get; set; }
    }

    public enum UserRoleType
    {
        Reviewer,
        Editor
    }
}
