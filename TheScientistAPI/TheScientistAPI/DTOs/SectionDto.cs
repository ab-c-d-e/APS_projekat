using System.Text.Json.Serialization;
using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public abstract class SectionDto
    {
        public int PaperId { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; }
        public SectionType Type { get; set; }
        public List<SectionDto>? SubSections { get; set; }
    }

    public class TextSectionDto : SectionDto
    {
        public List<string> Paragraphs { get; set; }

    }

    public class ImageSectionDto : SectionDto
    {
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class CodeSectionDto : SectionDto
    {
        public string Language { get; set; }
        public string Code { get; set; }

    }

    public class ImageSectionResponseDto:SectionDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class CodeSectionResponseDto:SectionDto
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
    }

    public class TextSectionResponseDto:SectionDto
    {
        public int Id { get; set; }
        public List<string> Paragraphs { get; set; }
    }

    public static partial class SectionExtension
    {
        public static SectionDto AsDto(this Section section)
        {
            if (section.Type == SectionType.Text)
            {
                return new TextSectionResponseDto
                {
                    Id = section.Id,
                    Title = section.Title,
                    Type = section.Type,
                    Paragraphs = section.Content?.Split('\n').ToList(), 
                    SubSections = section.Subsections?.Select(s=>s.AsDto()).ToList()
                };
            }
            else if (section.Type == SectionType.Code)
            {
                return new CodeSectionResponseDto
                {
                    Id = section.Id,
                    Title = section.Title,
                    Type = section.Type,
                    Code = section.Content,
                    SubSections = section.Subsections?.Select(s => s.AsDto()).ToList()
                };
            }
            else if (section.Type == SectionType.Image)
            {
                return new ImageSectionResponseDto
                {
                    Id = section.Id,
                    Title = section.Title,
                    Type = section.Type,
                    Url = section.Url,
                    Description=section.Content,
                    SubSections = section.Subsections?.Select(s => s.AsDto()).ToList()
                };
            }
            else throw new Exception("Not a valid type");
        }
    }
}
