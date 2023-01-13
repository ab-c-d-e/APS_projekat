using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheScientistAPI.Configuration;
using TheScientistAPI.Model;

namespace TheScientistAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ScientificPaperController : Controller
    {
        private readonly ILogger<ScientificPaperController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ScientificPaperController(ILogger<ScientificPaperController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("CreateScientificPaper")]
        public async Task<IActionResult> CreatePaper(ScientificPaper scientificPaper)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ScientificPapers.Add(scientificPaper);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetScientificPaper), new { scientificPaper.ID }, scientificPaper);
            }

            return new JsonResult("Data you entered is wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScientificPaper(int id)
        {
            var paper = await _unitOfWork.ScientificPapers.GetById(id);
            if (paper == null) return NotFound();
            return Ok(paper);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdatePaper(ScientificPaper scientificPaper)
        {
            await _unitOfWork.ScientificPapers.Upsert(scientificPaper);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScientificPaper(int id)
        {
            var paper = await _unitOfWork.ScientificPapers.GetById(id);
            if (paper == null) return NotFound();
            await _unitOfWork.ScientificPapers.Delete(id);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }
    }
}
