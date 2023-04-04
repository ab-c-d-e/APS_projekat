using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class ReferenceCreateDto
    {
        public int? Id { get; set; }
        public int PaperId { get; set; }
        public string? Title { get; set; }
        public List<string>? Authors { get; set; }
        public string? Journal { get; set; }
        public int? Year { get; set; }
        public int? LinkedPaperId { get; set; }
    }

    public static partial class ReferenceExtension
    {
        public static ReferenceDto AsDto(this Reference reference)
        {
            return new ReferenceDto
            {
                Id = reference.Id,
                Text = reference.Title + ", " + reference.Journal + ", " + reference.Year + " (" + string.Join(", ", reference.Authors.Select(a=>a.Name).ToList()) + ")",
                LinkedPaperId = reference.LinkedPaperId
            };
        }
    }
}
