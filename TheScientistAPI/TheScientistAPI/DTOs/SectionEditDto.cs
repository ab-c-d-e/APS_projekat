using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class SectionEditDto
    {
        public int Id { get; set; }
        public int PaperId { get; set; }
        public string Title { get; set; }
        public SectionType Type { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
    }
}
