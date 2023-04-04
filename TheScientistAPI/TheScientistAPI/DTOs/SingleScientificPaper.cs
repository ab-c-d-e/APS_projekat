using TheScientistAPI.Model;
using static System.Collections.Specialized.BitVector32;

namespace TheScientistAPI.DTOs
{
    public class SingleScientificPaper
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Journal { get; set; }
        public bool IsPublic { get; set; }
        public PaperStatus Status { get; set; }

        public string CreatorName { get; set; }

        public string CreatorId { get; set; } 

        public List<UserDto> Editors { get; set; }
        public List<UserDto> Reviewers { get; set; }
        public List<KeywordDto> Keywords { get; set;}
        public List<ReferenceDto> References { get; set; }
        public List<SectionDto> Sections { get; set; }
    }

    public class ReferenceDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int LinkedPaperId { get; set; }
    }


    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
    public static partial class ScientificPaperExtension
    {
        public static SingleScientificPaper AsPublicDto(this ScientificPaper paper)
        {
            return new SingleScientificPaper
            {
                Id = paper.Id,
                Title = paper.Title,
                Abstract = paper.Abstract,
                Journal = paper.Journal,
                IsPublic = paper.IsPublic,
                Status = paper.Status,
                Editors = paper.UserRoles.Where(us => us.RoleType== UserRoleType.Editor)
                    .Select(e=> new UserDto 
                    {
                        Id=e.User.Id,
                        Name=e.User.FirstName + " " + e.User.LastName ,
                        Email=e.User.Email,
                        UserName=e.User.UserName
                    }).ToList(),
                Reviewers = paper.UserRoles.Where(us => us.RoleType == UserRoleType.Reviewer)
                    .Select(e => new UserDto
                    {
                        Id = e.User.Id,
                        Name = e.User.FirstName + " " + e.User.LastName,
                        Email = e.User.Email,
                        UserName = e.User.UserName
                    }).ToList(),
                Keywords = paper.Keywords.Select(keyword => new KeywordDto
                {
                    Id = keyword.Id,
                    Name = keyword.Name
                }).ToList(),
                References = paper.References.Select(reference => new ReferenceDto
                {
                    Id = reference.Id,
                    Text = reference.Title + ", " + reference.Journal + ", " + reference.Year + " (" + string.Join(", ", reference.Authors.Select(a=>a.Name).ToList()) + ")",
                    LinkedPaperId = reference.LinkedPaperId
                }).ToList(),
                Sections = paper.Sections.Select(s => s.AsDto()).ToList(),
                CreatorName = paper.Creator.FirstName +" "+ paper.Creator.LastName,
                CreatorId=paper.Creator.Id
            };
        }
    }
}
