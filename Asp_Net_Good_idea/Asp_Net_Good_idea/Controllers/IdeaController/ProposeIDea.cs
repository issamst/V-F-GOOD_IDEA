using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.idea;
using Asp_Net_Good_idea.Models.Dto;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using System.IO.Compression;
using Asp_Net_Good_idea.Service.Ideas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Controllers.Idea
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdeaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IdeasService _ideasService;

        public IdeaController(AppDbContext context, IWebHostEnvironment environment, IdeasService ideasService)
        {
            _context = context;
            _environment = environment;
            _ideasService = ideasService;
        }

        [HttpPost]
        public async Task<ActionResult<Idea_M>> CreateIdea([FromBody] Idea_M idea)
        {
            if (ModelState.IsValid)
            {
                idea.Date_Create = DateTime.Now;
                idea.Status = "Pending";
                _context.Idea.Add(idea);

                await _context.SaveChangesAsync();
                return Ok(new { Message = "done !!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("ModelState Errors: " + string.Join(", ", errors));
            return BadRequest(new { Errors = errors });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Idea_M>>> GetAllIdeas()
        {
            return await _context.Idea.Include(d => d.TEID).ToListAsync();
        }

        [HttpGet("teid/{userId}")]
        public ActionResult<IEnumerable<Idea_M>> GetIdeasByUserId(string userId)
        {
            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid user id");
            }

            var ideas = _context.Idea.Where(i => i.UserId == userIdInt).Include(d => d.TEID).ToList();
            if (ideas == null || ideas.Count == 0)
            {
                return NotFound();
            }
            return Ok(ideas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIdea(int id, [FromBody] Idea_M idea)
        {
            var result = await _ideasService.UpdateIdea(id, idea);
            return result;
        }

        [HttpDelete("DisabledIdea/{id}/{commenter}")]
        public async Task<IActionResult> DisabledIdea(int id, string commenter)
        {
            var result = await _ideasService.DisableIdea(id, commenter);
            return result;
        }

        [HttpPost("FileSituation")]
        public async Task<ActionResult<List<string>>> UploadFileSituation(List<IFormFile> files)
        {
            var result = await _ideasService.UploadFiles(files, _environment, "FileSituation");
            return result;
        }

        [HttpPost("FileSolution")]
        public async Task<ActionResult<List<string>>> UploadFileSolution(List<IFormFile> files)
        {
            var result = await _ideasService.UploadFiles(files, _environment, "FileSolution");
            return result;
        }

        [HttpPost("FileIdea")]
        public async Task<ActionResult<List<string>>> UploadFileIdea(List<IFormFile> files)
        {
            var result = await _ideasService.UploadFiles(files, _environment, "FileIdea");
            return result;
        }

        [HttpGet("FileIdea/{fileName}")]
        public IActionResult GetFileIdea(string fileName)
        {
            var result = _ideasService.GetFile(fileName, _environment, "FileIdea");
            return result;
        }

        [HttpGet("FileSituation/{fileName}")]
        public IActionResult GetFileSituation(string fileName)
        {
            var result = _ideasService.GetFile(fileName, _environment, "FileSituation");
            return result;
        }

        [HttpGet("FileSolution/{fileName}")]
        public IActionResult GetFileSolution(string fileName)
        {
            var result = _ideasService.GetFile(fileName, _environment, "FileSolution");
            return result;
        }

        [HttpGet("ideaByid/{id}")]
        public async Task<ActionResult<Idea_M>> GetIdeaById(int id)
        {
            var idea = await _context.Idea.Include(d => d.TEID).FirstOrDefaultAsync(i => i.Id == id);

            if (idea == null)
            {
                return NotFound();
            }

            return Ok(idea);
        }

        [HttpPost("Team_Leader/Answer/{id}/{userId}/{response}/{comment}")]
        public async Task<IActionResult> TeamLeaderAnswer(int id, int userId, string response, string comment)
        {
            var result = await _ideasService.TeamLeaderAnswer(id, userId, response, comment);
            return result;
        }

        [HttpPost("Committee/Answer/{id}/{userId}/{response}/{comment}")]
        public async Task<IActionResult> CommitteeAnswer(int id, int userId, string response, string comment)
        {
            var result = await _ideasService.CommitteeAnswer(id, userId, response, comment);
            return result;
        }

        private bool IdeaExists(int id)
        {
            return _context.Idea.Any(e => e.Id == id);
        }
    }
}
