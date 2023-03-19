using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class ScientificPaperEditDto
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public List<string> Keywords { get; set; }
    }

    public class ScientificPaperEditResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public List<KeywordDto> Keywords { get; set; }
    }

    public class KeywordDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ScientificPaperUserDto
    {
        public string UserName { get; set; }
        public UserRoleType Role { get; set; }
    }
}
