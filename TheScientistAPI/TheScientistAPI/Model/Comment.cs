using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }
        [JsonIgnore]
        public Section Section { get; set; }
        [JsonIgnore]
        public List<Comment> SubComments { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
