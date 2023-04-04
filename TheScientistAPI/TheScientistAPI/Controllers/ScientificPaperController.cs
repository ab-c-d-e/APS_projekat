using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheScientistAPI.Configuration;
using TheScientistAPI.DTOs;
using TheScientistAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Data;
using TheScientistAPI.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace TheScientistAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ScientificPaperController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ScientistHub> _hubContext;

        public ScientificPaperController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHubContext<ScientistHub> hubContext)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateScientificPaper([FromBody] ScientificPaperDto scientificPaperDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = new ScientificPaper
            {
                Title = scientificPaperDto.Title,
                Abstract = scientificPaperDto.Abstract,
                Journal = scientificPaperDto.Journal,
                IsPublic = scientificPaperDto.IsPublic,
                Status = PaperStatus.Active,
                Keywords = new List<Keyword>(),
                References = new List<Reference>(),
                Sections = new List<Section>(),
                UserRoles=new List<UserRole>(),
                Messages = new List<Message>(),
                Creator = user
            };

            foreach (var keyword in scientificPaperDto.Keywords)
            {
                if (!scientificPaper.Keywords.Select(k => k.Name).Contains(keyword))
                {
                    var kw1 = new Keyword { Name = keyword };
                    var kw2 = _unitOfWork.Keywords.GetByName(keyword);
                    if (kw2 == null)
                    {
                        scientificPaper.Keywords.Add(kw1);
                    }
                    else
                    {
                        scientificPaper.Keywords.Add(kw2);
                    }
                }
            }

            _unitOfWork.ScientificPapers.Add(scientificPaper);
            await _unitOfWork.CompleteAsync();

            var connection = _hubContext.Clients.Group(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await connection.SendAsync("ReceiveMessage", "New Scientific Paper Created: " + scientificPaper.Title);

            return Ok(scientificPaper.AsDto());
        }

        [HttpPut("edit")]
        public async Task<IActionResult> UpdateScientificPaper([FromBody] ScientificPaperEditDto scientificPaperEditDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(scientificPaperEditDto.Id, true, false, false, false, false);

            if (scientificPaper == null)
            {
                return NotFound();
            }

            if (scientificPaper.Creator != user)
            {
                return Unauthorized();
            }

            scientificPaper.Title = scientificPaperEditDto.Title;
            scientificPaper.Abstract = scientificPaperEditDto.Abstract;
            scientificPaper.Journal = scientificPaperEditDto.Journal;
            scientificPaper.IsPublic = scientificPaperEditDto.IsPublic;
            scientificPaper.Status = scientificPaperEditDto.Status;

            if (scientificPaper.Status == PaperStatus.Published)
                scientificPaper.Year = DateTime.Now.Year;

            var keywordsToRemove = new List<Keyword>();
            foreach (var keyword in scientificPaper.Keywords)
            {
                if (!scientificPaperEditDto.Keywords.Contains(keyword.Name))
                    keywordsToRemove.Add(keyword);
            }

            foreach (var keywordToRemove in keywordsToRemove)
            {
                scientificPaper.Keywords.Remove(keywordToRemove);
            }

            foreach (var keyword in scientificPaperEditDto.Keywords)
            {
                if (!scientificPaper.Keywords.Select(k => k.Name).Contains(keyword))
                {
                    var kw1=new Keyword { Name = keyword };
                    var kw2 = _unitOfWork.Keywords.GetByName(keyword);
                    if (kw2 == null)
                    {
                        scientificPaper.Keywords.Add(kw1);
                    }
                    else
                    {
                        scientificPaper.Keywords.Add(kw2);
                    }
                }
            }

            _unitOfWork.ScientificPapers.Update(scientificPaper);
            await _unitOfWork.CompleteAsync();

            var newState = scientificPaper.AsDto();

            await _hubContext.Clients.Group(scientificPaper.Id.ToString()).SendAsync("EditedPaper", newState);
            return Ok(scientificPaper.AsDto());
        }

        [HttpPost("addUsers")]
        public async Task<IActionResult> AddUserToScientificPaper([FromBody] AddUserDto userDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(userDto.PaperId, true, true, false, false, false);
            if (scientificPaper == null)
            {
                return NotFound();
            }

            if (scientificPaper.Creator != user)
            {
                return Unauthorized();
            }

            var userToAdd = await _userManager.Users.Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserName == userDto.UserName);

            if (userToAdd == null)
            {
                return BadRequest("User with provided UserName not found.");
            }

            if (userToAdd == user)
            {
                return BadRequest("Creator has the highest role provided.");
            }

            var existingUserRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == userToAdd);
            if (existingUserRole != null)
            {
                return BadRequest("User is already added to the scientific paper with provided role.");
            }

            if (userDto.Role != UserRoleType.Editor && userDto.Role != UserRoleType.Reviewer)
            {
                return BadRequest("Invalid Role.");
            }

            var userRole = new UserRole
            {
                User = userToAdd,
                RoleType = userDto.Role,
                ScientificPaper = scientificPaper
            };

            _unitOfWork.UserRoles.Add(userRole);
            await _unitOfWork.CompleteAsync();

            userToAdd.UserRoles.Add(userRole);
            scientificPaper.UserRoles.Add(userRole);

            await _unitOfWork.CompleteAsync();

            var u = new ApplicationUser
            {
                Id = userRole.User.Id,
                Name = userRole.User.FirstName + " " + userRole.User.LastName,
                Email = userRole.User.Email,
                UserName = userRole.User.UserName
            };
            userRole.User = u;
            await _hubContext.Clients.Group(scientificPaper.Id.ToString()).SendAsync("NewUser", userRole);
            await _hubContext.Clients.Group(userToAdd.Email.ToString()).SendAsync("AddedAsRole", scientificPaper.AsDto());
            return Ok(userRole);
        }

        [HttpPost("addTextSection")]
        public async Task<IActionResult> AddSectionToScientificPaper([FromBody] TextSectionDto textSectionDto)
        {
            try
            {
                // Validation
                if (textSectionDto == null)
                {
                    return BadRequest("Invalid request body.");
                }

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByEmailAsync(userId);

                // Section creation
                Section section = new Section
                {
                    Title = textSectionDto.Title,
                    Type = SectionType.Text,
                    Content = string.Join("\n", textSectionDto.Paragraphs),
                    Status = SectionStatus.Active
                };

                if (textSectionDto.PaperId!=0)
                {
                    var scientificPaper = _unitOfWork.ScientificPapers.GetById(textSectionDto.PaperId, false, true, true, false, false);
                    if (scientificPaper == null)
                    {
                        return NotFound();
                    }

                    var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
                    if (user != scientificPaper.Creator && (userRole == null || userRole.RoleType == UserRoleType.Reviewer))
                    {
                        return Unauthorized();
                    }

                    scientificPaper.Sections.Add(section);
                    section.Paper = scientificPaper;
                    await _unitOfWork.CompleteAsync();
                    await NotifyClients(scientificPaper.Id.ToString(), section.AsDto());
                }
                else if(textSectionDto.SectionId != 0)
                {
                    var sec = _unitOfWork.Sections.GetAllIncluding(s=>s.Paper).ToList();
                    var parentSection = _unitOfWork.Sections.GetById(textSectionDto.SectionId, true);
                    if (parentSection == null)
                    {
                        return NotFound();
                    }
                    section.Paper = parentSection.Paper;

                    parentSection.Subsections.Add(section);
                    await _unitOfWork.CompleteAsync();

                    await NotifyClients(parentSection.Paper.Id.ToString(), section.AsDto());
                }

                return Ok(section.AsDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addCodeSection")]
        public async Task<IActionResult> AddSectionToScientificPaper([FromBody] CodeSectionDto codeSectionDto)
        {
            try
            {
                // Validation
                if (codeSectionDto == null)
                {
                    return BadRequest("Invalid request body.");
                }

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByEmailAsync(userId);

                // Section creation
                Section section = new Section
                        {
                            Title = codeSectionDto.Title,
                            Type = SectionType.Code,
                            Content = codeSectionDto.Code,
                            Language = codeSectionDto.Language,
                            Status = SectionStatus.Active
                        };

                if (codeSectionDto.PaperId != 0)
                {
                    var scientificPaper = _unitOfWork.ScientificPapers.GetById(codeSectionDto.PaperId, false, true, true, false, false);
                    if (scientificPaper == null)
                    {
                        return NotFound();
                    }

                    var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
                    if (user != scientificPaper.Creator && (userRole == null || userRole.RoleType == UserRoleType.Reviewer))
                    {
                        return Unauthorized();
                    }
                    section.Paper = scientificPaper;
                    scientificPaper.Sections.Add(section);
                    await _unitOfWork.CompleteAsync();
                    await NotifyClients(scientificPaper.Id.ToString(), section.AsDto());
                }
                else if (codeSectionDto.SectionId != 0)
                {
                    var parentSection = _unitOfWork.Sections.GetById(codeSectionDto.SectionId, true);
                    if (parentSection == null)
                    {
                        return NotFound();
                    }

                    section.Paper = parentSection.Paper;
                    parentSection.Subsections.Add(section);
                    await _unitOfWork.CompleteAsync();

                    await NotifyClients(parentSection.Paper.Id.ToString(), section.AsDto());
                }

                return Ok(section.AsDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addImageSection")]
        public async Task<IActionResult> AddSectionToScientificPaper([FromBody] ImageSectionDto imageSectionDto)
        {
            try
            {
                // Validation
                if (imageSectionDto == null)
                {
                    return BadRequest("Invalid request body.");
                }

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByEmailAsync(userId);

                // Section creation
                Section section = new Section
                {
                    Title = imageSectionDto.Title,
                    Type = SectionType.Image,
                    Url = imageSectionDto.Url,
                    Content = imageSectionDto.Description,
                    Status = SectionStatus.Active
                };

                if (imageSectionDto.PaperId != 0)
                {
                    var scientificPaper = _unitOfWork.ScientificPapers.GetById(imageSectionDto.PaperId, false, true, true, false, false);
                    if (scientificPaper == null)
                    {
                        return NotFound();
                    }

                    var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
                    if (user != scientificPaper.Creator && (userRole == null || userRole.RoleType == UserRoleType.Reviewer))
                    {
                        return Unauthorized();
                    }
                    section.Paper = scientificPaper;
                    scientificPaper.Sections.Add(section);
                    await _unitOfWork.CompleteAsync();
                    await NotifyClients(scientificPaper.Id.ToString(), section.AsDto());
                }
                else if (imageSectionDto.SectionId != 0)
                {
                    var parentSection = _unitOfWork.Sections.GetById(imageSectionDto.SectionId, true);
                    if (parentSection == null)
                    {
                        return NotFound();
                    }

                    section.Paper = parentSection.Paper;
                    parentSection.Subsections.Add(section);
                    await _unitOfWork.CompleteAsync();

                    await NotifyClients(parentSection.Paper.Id.ToString(), section.AsDto());
                }

                return Ok(section.AsDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private async Task NotifyClients(string scientificPaperId, SectionDto sectionDto)
        {
            await _hubContext.Clients.Group(scientificPaperId).SendAsync("SectionAdded", sectionDto);
        }

        [HttpPost("addNewReference")]
        public async Task<IActionResult> AddReference([FromBody] ReferenceCreateDto referenceDto)
        {
            var scientificPaper = _unitOfWork.ScientificPapers.GetById(referenceDto.PaperId, false, false, false, true, false);
            if (scientificPaper == null)
            {
                return NotFound();
            }
            if(referenceDto.Id!=null)
            {
                var reference = _unitOfWork.References.GetByIdWithAuthors((int)referenceDto.Id);
                if (reference == null)
                    return BadRequest("Not a valid reference to link to");
                scientificPaper.References.Add(reference);
                await _unitOfWork.CompleteAsync();
                await _hubContext.Clients.Group(scientificPaper.Id.ToString()).SendAsync("EditedPaper", reference.AsDto());
                return Ok(reference.AsDto());
            }
            else if (referenceDto.LinkedPaperId != null)
            {
                var paper = _unitOfWork.ScientificPapers.GetById(((int)referenceDto.LinkedPaperId), false, true, false, false, false);
                if (paper == null)
                    return BadRequest("Not a valid paper to link reference to");
                var reference = new Reference
                {
                    Title = paper.Title,
                    Journal = paper.Journal,
                    Year = paper.Year,
                    LinkedPaperId = paper.Id
                };
                reference.Authors = new List<Author>
                {
                   new Author {Name=paper.Creator.FirstName + " " + paper.Creator.LastName }
                };
                foreach(var user in paper.UserRoles)
                {
                    if (user.RoleType == UserRoleType.Editor)
                        reference.Authors.Add(new Author { Name=user.User.FirstName + " " + user.User.LastName });
                }
                scientificPaper.References.Add(reference);
                await _unitOfWork.CompleteAsync();
                await _hubContext.Clients.Group(scientificPaper.Id.ToString()).SendAsync("EditedPaper", reference.AsDto());
                return Ok(reference.AsDto());
            }
            else
            {
                var reference = new Reference
                {
                    Title = referenceDto.Title,
                    Journal = referenceDto.Journal,
                    Year = (int)referenceDto.Year,
                    Authors = referenceDto.Authors.Select(a => new Author { Name = a }).ToList()
                };
                scientificPaper.References.Add(reference);
                await _unitOfWork.CompleteAsync();
                await _hubContext.Clients.Group(scientificPaper.Id.ToString()).SendAsync("EditedPaper", reference.AsDto());
                return Ok(reference.AsDto());
            }
        }

        [HttpGet("getAllUsersPapers")]
        public async Task<IActionResult> GetAllUsersPapers()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var papers = _unitOfWork.ScientificPapers.GetAllIncluding(p => p.Creator, p => p.UserRoles, p => p.Keywords);
            var myPapers = papers.Where(p => p.Creator.Id == user.Id || p.UserRoles.Any(ur => ur.User.Id == user.Id &&
                                       (ur.RoleType == UserRoleType.Editor || ur.RoleType == UserRoleType.Reviewer))).ToList();

            return Ok(myPapers.Select(paper=>paper.AsDto()).ToList());
        }

        [HttpGet("getAllPublicPapers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPublicPapers()
        {
            var papers = _unitOfWork.ScientificPapers.GetAllIncluding(p => p.Creator, p => p.UserRoles, p => p.Keywords);
            var publicPapers = papers.Where(p => p.IsPublic).ToList();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            if (user!=null)
            {
                var myPapers = papers.Where(p => p.Creator.Id == user.Id || p.UserRoles.Any(ur => ur.User.Id == user.Id &&
                                           (ur.RoleType == UserRoleType.Editor || ur.RoleType == UserRoleType.Reviewer))).ToList();
                foreach(var paper in myPapers)
                {
                    if(!papers.Contains(paper))
                    {
                        publicPapers.Add(paper);
                    }
                }
            }

            return Ok(publicPapers.Select(paper=>paper.AsDto()));
        }

        [HttpGet("getByKeywords")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchPapersByKeywords([FromQuery] List<string> keywords)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var papers = _unitOfWork.ScientificPapers.GetAllIncluding(sp=>sp.Keywords, sp=>sp.Creator)
                .Where(p => p.Keywords.Any(k => keywords.Contains(k.Name)))
                .ToList();

            return Ok(papers.Select(p=>p.AsDto()));
        }

        [HttpGet("getByReferences/{SearchTerm}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchPapersForReferences(string SearchTerm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var papers = _unitOfWork.ScientificPapers.GetAllIncluding(p => p.References, p=>p.UserRoles, p=>p.Keywords);
            var matchingPapers = papers.Where(p => p.References.Any(r =>
                r.Title.Contains(SearchTerm) ||
                r.Authors.Any(a => a.Name.Contains(SearchTerm)) ||
                r.Journal.Contains(SearchTerm) ||
                r.Year.ToString().Contains(SearchTerm)
                )).ToList();

            return Ok(matchingPapers.Select(paper=>paper.AsDto()));
        }

        [HttpGet("getByReference/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchPapersForReference(int Id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var papers = _unitOfWork.ScientificPapers.GetAllIncluding(p => p.References);
            var matchingPapers = papers.Where(p => p.References.Any(r => r.Id == Id))
                .ToList();

            return Ok(matchingPapers);
        }

        [HttpGet("getFullPublicPaper/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFullPublicPaper(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, true, true, true, true, false);
            if (scientificPaper == null)
            {
                return NotFound();
            }
            if (!scientificPaper.IsPublic)
            {
                return Unauthorized();
            }
            var sections = scientificPaper.Sections.Where(section => section.Status == SectionStatus.Finnished).ToList();
            scientificPaper.Sections = sections;

            return Ok(scientificPaper.AsPublicDto());
        }

        [HttpGet("getFullEditablePaper/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFullEditablePaper(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, true, true, true, true, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }

           return Ok(scientificPaper.AsPublicDto());
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var scientificPaper = _unitOfWork.ScientificPapers.GetById(messageDto.PaperId, false, true, false, false, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }
            var message = new Message
            {
                Text = messageDto.Text,
                User = user
            };
            scientificPaper.Messages.Add(message);
            await _unitOfWork.CompleteAsync();
            var messageUser = new MessageUser
            {
                Message = message,
                User = scientificPaper.Creator,
                Seen = scientificPaper.Creator==user
            };
            _unitOfWork.MessageUsers.Add(messageUser);
            await _unitOfWork.CompleteAsync();
            foreach (var usr in scientificPaper.UserRoles)
            {
               messageUser = new MessageUser
                {
                    Message = message,
                    User = usr.User,
                    Seen = usr.User == user
                };
                _unitOfWork.MessageUsers.Add(messageUser);
                await _unitOfWork.CompleteAsync();
            }
            await _hubContext.Clients.Group(scientificPaper.Id.ToString()).SendAsync("NewMessage", message.AsDto());

            return Ok(message.AsDto());
        }

        [HttpPost("getAllMessages/{Id}")]
        public async Task<IActionResult> GetAllMessages(int Id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var scientificPaper = _unitOfWork.ScientificPapers.GetById(Id, false, true, false, false, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }
            var messages = _unitOfWork.MessageUsers.GetByUserAndPaper(user.Id, Id);
            foreach(var message in messages)
            {
                message.Seen = true;
            }
            await _unitOfWork.CompleteAsync();
            return Ok(messages.Select(m=>m.Message.AsDto()));
        }

        [HttpGet("getAllUnseenMessages/{Id}")]
        public async Task<IActionResult> GetAllUnseenMessages(int Id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);
            var scientificPaper = _unitOfWork.ScientificPapers.GetById(Id, false, true, false, false, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }
            var messages = _unitOfWork.MessageUsers.GetByUserAndPaper(user.Id, Id);
            var seenMessages = messages.Where(m => !m.Seen).ToList();
            await _unitOfWork.CompleteAsync();
            return Ok(seenMessages.Select(m => m.Message.AsDto()));
        }

        [AllowAnonymous]
        [HttpGet("getAllKeywords")]
        public async Task<IActionResult> GetAllKeywords()
        {
            var keywords = _unitOfWork.Keywords.GetAll();
            return Ok(keywords);
        }

        [AllowAnonymous]
        [HttpGet("getAllReferences")]
        public async Task<IActionResult> GetAllReferences()
        {
            var references = _unitOfWork.References.GetAllIncluding(p=>p.Authors);
            return Ok(references.Select(k=>k.AsDto()));
        }

        //[HttpPost("updateSection/{id}")]
        //public async Task<IActionResult> UpdateSectionToScientificPaper(int id, [FromBody] SectionEditDto sectionEditDto)
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var user = await _userManager.FindByEmailAsync(userId);

        //    var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, false, true, true, false);
        //    if (scientificPaper == null)
        //    {
        //        return NotFound();
        //    }

        //    var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
        //    if (user != scientificPaper.Creator || userRole == null || userRole.RoleType == UserRoleType.Reviewer)
        //    {
        //        return Unauthorized();
        //    }

        //    if (sectionEditDto.PaperId != scientificPaper.Id)
        //    {
        //        return BadRequest("Does not belong to the right paper.");
        //    }

        //    if (sectionEditDto.Type != SectionType.Code && sectionEditDto.Type != SectionType.Image && sectionEditDto.Type != SectionType.Text)
        //    {
        //        return BadRequest("Invalid Type.");
        //    }

        //    var section = _unitOfWork.Sections.GetById(sectionEditDto.Id);
        //    section.Title = sectionEditDto.Title;
        //    section.Content = sectionEditDto.Content;
        //    section.Url = sectionEditDto.Content;

        //    _unitOfWork.Sections.Update(section);
        //    await _unitOfWork.CompleteAsync();

        //    var userIds = new List<string>();
        //    userIds.Add(scientificPaper.Creator.Id);
        //    foreach (var u in scientificPaper.UserRoles) userIds.Add(u.User.Id);

        //    return Ok(section);
        //}

        //[HttpGet("getSections/{id}")]
        //public async Task<IActionResult> GetSectionsOfScientificPaper(int id)
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var user = await _userManager.FindByEmailAsync(userId);

        //    var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, false, true, true, false);
        //    if (scientificPaper == null)
        //    {
        //        return NotFound();
        //    }

        //    var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
        //    if (user != scientificPaper.Creator || userRole == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(scientificPaper.Sections);
        //}
    }
}
