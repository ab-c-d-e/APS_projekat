using System.ComponentModel.DataAnnotations;
using TheScientistAPI.Model;

namespace TheScientistAPI.DTOs
{
    public class SectionCreateDto
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public SectionType Type { get; set; }

        [MaxLength(200)]
        public string Url { get; set; }

        public string Content { get; set; }
    }

    public class SectionDto
    {
        public int Id { get; set; }
        public int PaperId { get; set; }
        public string Title { get; set; }
        public SectionType Type { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
    }

    public class SectionPreviousState
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
    }
}
