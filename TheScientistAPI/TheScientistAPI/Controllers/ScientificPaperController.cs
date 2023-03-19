using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheScientistAPI.Configuration;
using TheScientistAPI.DTOs;
using TheScientistAPI.Model;
using TheScientistAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace TheScientistAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ScientificPaperController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScientificPaperController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) :base(userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateScientificPaper([FromBody] ScientificPaperDTO scientificPaperDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = new ScientificPaper
            {
                Title = scientificPaperDto.Title,
                Abstract = scientificPaperDto.Abstract,
                Keywords = new List<Keyword>(),
                References=new List<Reference>(),
                Sections=new List<Section>(),
                Creator = user
            };

            _unitOfWork.ScientificPapers.Add(scientificPaper);
            await _unitOfWork.CompleteAsync();

            return Ok(scientificPaper);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateScientificPaper(int id, ScientificPaperEditDto scientificPaperUpdateDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id,true) ;
            if (scientificPaper == null)
            {
                return NotFound();
            }

            if (scientificPaper.Creator != user)
            {
                return Unauthorized();
            }

            scientificPaper.Title = scientificPaperUpdateDto.Title;
            scientificPaper.Abstract = scientificPaperUpdateDto.Abstract;

            // Update keywords
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

            return NoContent();
        }

        [HttpPost("addUsers/{id}/users")]
        public async Task<IActionResult> AddUserToScientificPaper(int id, ScientificPaperUserDto scientificPaperUserDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = await _userManager.FindByEmailAsync(userId);

            var scientificPaper = _unitOfWork.ScientificPapers.GetById(id, true);
            if (scientificPaper == null)
            {
                return NotFound();
            }

            if (scientificPaper.Creator!= user)
            {
                return Unauthorized();
            }

            var userToAdd = await _userManager.FindByNameAsync(scientificPaperUserDto.UserName);
            if (userToAdd == null)
            {
                return BadRequest("User with provided UserName not found.");
            }

            var existingUserRole = scientificPaper.UserRoles.FirstOrDefault(ur => ur.User == userToAdd);
            if (existingUserRole != null)
            {
                return BadRequest("User is already added to the scientific paper with provided role.");
            }

            var userRole = new UserRole
            {
                User = userToAdd,
                RoleType = scientificPaperUserDto.Role,
                ScientificPaper = scientificPaper
            };

            _unitOfWork.UserRoles.Add(userRole);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
