using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheScientistAPI.Configuration;
using TheScientistAPI.DTOs;
using TheScientistAPI.Model;
using TheScientistAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Data;
using Microsoft.AspNetCore.SignalR;
using TheScientistAPI.SignalR;

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
        public async Task<IActionResult> CreateScientificPaper([FromBody] ScientificPaperDTO scientificPaperDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = new ScientificPaper
            {
                Title = scientificPaperDto.Title,
                Abstract = scientificPaperDto.Abstract,
                Keywords = new List<Keyword>(),
                References = new List<Reference>(),
                Sections = new List<Section>(),
                Creator = user
            };

            _unitOfWork.ScientificPapers.Add(scientificPaper);
            await _unitOfWork.CompleteAsync();

            return Ok(scientificPaper);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateScientificPaper(int id, [FromBody] ScientificPaperEditDto scientificPaperUpdateDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, true, false, false);
            if (scientificPaper == null)
            {
                return NotFound();
            }

            if (scientificPaper.Creator != user)
            {
                return Unauthorized();
            }
            var previousState = new ScientificPaperMemento(scientificPaper);

            scientificPaper.Title = scientificPaperUpdateDto.Title;
            scientificPaper.Abstract = scientificPaperUpdateDto.Abstract;

            var keywordsToAdd = scientificPaperUpdateDto.Keywords
                .Except(scientificPaper.Keywords.Select(k => k.Name), StringComparer.OrdinalIgnoreCase)
                .Select(name => new Keyword { Name = name });
            scientificPaper.Keywords.AddRange(keywordsToAdd);

            var keywordsToRemove = scientificPaper.Keywords
                .Where(k => !scientificPaperUpdateDto.Keywords.Contains(k.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();
            keywordsToRemove.ForEach(k => scientificPaper.Keywords.Remove(k));
            //Reminder delete them from Database
            _unitOfWork.ScientificPapers.Update(scientificPaper);
            await _unitOfWork.CompleteAsync();

            var newState = new ScientificPaperMemento(scientificPaper);
            var message = new ScientificPaperEditMessage
            {
                PreviousState = previousState,
                NewState = newState,
                EditorName = user.UserName
            };
            await _hubContext.Clients.Group(scientificPaper.Id.ToString()).SendAsync("EditScientificPaper", message);

            return Ok(scientificPaper);
        }

        [HttpPost("addUsers/{id}")]
        public async Task<IActionResult> AddUserToScientificPaper(int id, [FromBody] ScientificPaperUserDto scientificPaperUserDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, false, true, false);
            if (scientificPaper == null)
            {
                return NotFound();
            }

            if (scientificPaper.Creator != user)
            {
                return Unauthorized();
            }

            var userToAdd = await _userManager.FindByNameAsync(scientificPaperUserDto.UserName);
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

            if (scientificPaperUserDto.Role != UserRoleType.Editor && scientificPaperUserDto.Role != UserRoleType.Reviewer)
            {
                return BadRequest("Invalid Role.");
            }

            var userRole = new UserRole
            {
                User = userToAdd,
                RoleType = scientificPaperUserDto.Role,
                ScientificPaper = scientificPaper
            };

            _unitOfWork.UserRoles.Add(userRole);
            await _unitOfWork.CompleteAsync();

            await _hubContext.Groups.AddToGroupAsync("1234",scientificPaper.Id.ToString());


            return Ok(scientificPaper);
        }

        [HttpPost("addSection/{id}")]
        public async Task<IActionResult> AddSectionToScientificPaper(int id, [FromBody] SectionCreateDto sectionCreateDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, false, true, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }
            var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
            if (user!=scientificPaper.Creator || userRole == null || userRole.RoleType== UserRoleType.Reviewer)
            {
                return Unauthorized();
            }

            if(sectionCreateDto.Type != SectionType.Code && sectionCreateDto.Type != SectionType.Image && sectionCreateDto.Type != SectionType.Text)
            {
                return BadRequest("Invalid Type.");
            }

            var section = new Section
            {
                Paper = scientificPaper,
                Title = sectionCreateDto.Title,
                Type = sectionCreateDto.Type,
                Url = sectionCreateDto.Url,
                Content = sectionCreateDto.Content
            };

            _unitOfWork.Sections.Add(section);
            await _unitOfWork.CompleteAsync();

            return Ok(section);
        }

        [HttpPost("updateSection/{id}")]
        public async Task<IActionResult> UpdateSectionToScientificPaper(int id,[FromBody] SectionEditDto sectionEditDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, false, true, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }

            var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
            if (user != scientificPaper.Creator || userRole == null || userRole.RoleType == UserRoleType.Reviewer)
            {
                return Unauthorized();
            }

            if (sectionEditDto.PaperId!= scientificPaper.Id)
            {
                return BadRequest("Does not belong to the right paper.");
            }

            if (sectionEditDto.Type != SectionType.Code && sectionEditDto.Type != SectionType.Image && sectionEditDto.Type != SectionType.Text)
            {
                return BadRequest("Invalid Type.");
            }

            var section = _unitOfWork.Sections.GetById(sectionEditDto.Id);
            section.Title = sectionEditDto.Title;
            section.Content = sectionEditDto.Content;
            section.Url = sectionEditDto.Content;

            _unitOfWork.Sections.Update(section);
            await _unitOfWork.CompleteAsync();

            var userIds = new List<string>();
            userIds.Add(scientificPaper.Creator.Id);
            foreach(var u in scientificPaper.UserRoles) userIds.Add(u.User.Id);

            await _hubContext.Clients.Users(userIds).SendAsync("EditScientificPaper", scientificPaper);

            return Ok(section);
        }

        [HttpGet("getSections/{id}")]
        public async Task<IActionResult> GetSectionsOfScientificPaper(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, false, true, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }

            var userRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == user);
            if (user != scientificPaper.Creator || userRole == null)
            {
                return Unauthorized();
            }

            return Ok(scientificPaper.Sections);
        }
        [HttpGet("getPaperUser")]
        public async Task<IActionResult> GetAllPaperOfUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(userId);

            var papers =_unitOfWork.ScientificPapers.GetAllIncluding(p => p.Creator, p => p.UserRoles);
            var myPapers = papers.Where(p => p.Creator.Id == user.Id || p.UserRoles.Any(ur => ur.User.Id == user.Id &&
                                       (ur.RoleType == UserRoleType.Editor || ur.RoleType == UserRoleType.Reviewer))).ToList();

            return Ok(myPapers);

        }
    }
}
