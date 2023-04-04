using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class ScientificPaperDto
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Journal { get; set; }
        public bool IsPublic { get; set; }
        public List<string> Keywords { get; set; }
    }

    public class ScientificPaperEditDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Journal { get; set; }
        public bool IsPublic { get; set; }
        public PaperStatus Status { get; set; }
        public List<string> Keywords { get; set; }
    }

    public class ScientificPaperResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Journal { get; set; }
        public int Year { get; set; }
        public bool IsPublic { get; set; }
        public PaperStatus Status { get; set; }
        public List<KeywordDto> Keywords { get; set; }
        public string Creator { get; set; }
    }

    public class KeywordDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static partial class ScientificPaperExtension
    {
        public static ScientificPaperResponseDto AsDto(this ScientificPaper paper)
        {
            return new ScientificPaperResponseDto
            {
                Id = paper.Id,
                Title = paper.Title,
                Abstract = paper.Abstract,
                Journal = paper.Journal,
                Year = paper.Year,
                IsPublic = paper.IsPublic,
                Status = paper.Status,
                Keywords = paper.Keywords.Select(keyword => new KeywordDto
                {
                    Id = keyword.Id,
                    Name = keyword.Name
                }).ToList(),
                Creator = paper.Creator.UserName
            };
        }
    }
}
