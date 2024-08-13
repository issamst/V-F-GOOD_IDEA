using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.idea;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace Asp_Net_Good_idea.Service.Ideas
{
    public class IdeasService
    {
        private readonly AppDbContext _context;

        public IdeasService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> UpdateIdea(int id, Idea_M idea)
        {
            try
            {
                var existingIdea = await _context.Idea.FirstOrDefaultAsync(x => x.Id == id);
                if (existingIdea != null)
                {
                    existingIdea.Title = idea.Title;
                    existingIdea.Description = idea.Description;
                    existingIdea.DescriptionSituation = idea.DescriptionSituation;
                    existingIdea.DescriptionSolution = idea.DescriptionSolution;
                    existingIdea.FileIdeaPath = idea.FileIdeaPath;
                    existingIdea.FileSolutionPath = idea.FileSolutionPath;
                    existingIdea.FileSituationPath = idea.FileSituationPath;
                    existingIdea.Name_Area = idea.Name_Area;
                    existingIdea.Name_Impact = idea.Name_Impact;
                    existingIdea.Name_Machine = idea.Name_Machine;
                    existingIdea.Project_Name = idea.Project_Name;
                    existingIdea.Date_Update = DateTime.Now;
                    await _context.SaveChangesAsync();

                    return new OkObjectResult(new { message = "Update successful!" });
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return new NotFoundObjectResult("Idea not found");
            }
        }

        public async Task<IActionResult> DisableIdea(int id, string commenter)
        {
            try
            {
                var success = await _context.Idea.FindAsync(id);
                if (success != null)
                {
                    success.Date_delete = DateTime.Now;
                    success.CommenterDisabled = commenter;
                    success.Status = "Disabled";
                    _context.Idea.Update(success);
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new { Message = "Idea disabled successfully" });
                }

                return new NotFoundObjectResult("Idea not found");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { ex.Message });
            }
        }

        public async Task<ActionResult<List<string>>> UploadFiles(List<IFormFile> files, IWebHostEnvironment environment, string folder)
        {
            if (files == null || files.Count == 0)
                return new BadRequestObjectResult("No files selected");

            var fileNames = new List<string>();
            foreach (var file in files)
            {
                string uniqueFileName = GenerateUniqueFileName(file.FileName);
                string directoryPath = Path.Combine(environment.ContentRootPath, "Upload", folder);
                string filePath = Path.Combine(directoryPath, uniqueFileName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                fileNames.Add(uniqueFileName);
            }

            return new OkObjectResult(new { FileNames = fileNames });
        }

        public IActionResult GetFile(string fileName, IWebHostEnvironment environment, string folder)
        {
            string filePath = Path.Combine(environment.ContentRootPath, "Upload", folder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return new NotFoundResult();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return new FileContentResult(fileBytes, contentType) { FileDownloadName = fileName };
        }

        public async Task<IActionResult> TeamLeaderAnswer(int id, int userId, string response, string comment)
        {
            var existingIdea = await _context.Idea.FirstOrDefaultAsync(x => x.Id == id);
            if (existingIdea == null)
            {
                return new NotFoundObjectResult("Idea not found");
            }

            if (response.Equals("approved", StringComparison.OrdinalIgnoreCase))
            {
                if (existingIdea.Team_Leader_Approved == null)
                {
                    existingIdea.Team_Leader_Approved = new List<int>();
                    existingIdea.CommenterTLApproved = comment;
                }
                if (!existingIdea.Team_Leader_Approved.Contains(userId))
                {
                    existingIdea.Team_Leader_Approved.Add(userId);
                    existingIdea.CommenterTLApproved = comment;
                }
            }
            else if (response.Equals("rejected", StringComparison.OrdinalIgnoreCase))
            {
                if (existingIdea.Team_Leader_Rejected == null)
                {
                    existingIdea.Team_Leader_Rejected = new List<int>();
                    existingIdea.CommenterTLRejected = comment;
                }
                if (!existingIdea.Team_Leader_Rejected.Contains(userId))
                {
                    existingIdea.Team_Leader_Rejected.Add(userId);
                    existingIdea.CommenterTLRejected = comment;
                }
            }
            else
            {
                return new BadRequestObjectResult("Invalid response");
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(new { Message = response.Equals("approved", StringComparison.OrdinalIgnoreCase) ? "Approved" : "Rejected" });
        }

        public async Task<IActionResult> CommitteeAnswer(int id, int userId, string response, string comment)
        {
            var existingIdea = await _context.Idea.FirstOrDefaultAsync(x => x.Id == id);
            if (existingIdea == null)
            {
                return new NotFoundObjectResult("Idea not found");
            }

            if (response.Equals("approved", StringComparison.OrdinalIgnoreCase))
            {
                if (existingIdea.Committee_Approved == null)
                {
                    existingIdea.Committee_Approved = new List<int>();
                    existingIdea.CommenterCOMApproved = comment;
                }
                if (!existingIdea.Committee_Approved.Contains(userId))
                {
                    existingIdea.Committee_Approved.Add(userId);
                    existingIdea.CommenterCOMApproved = comment;
                }
            }
            else if (response.Equals("rejected", StringComparison.OrdinalIgnoreCase))
            {
                if (existingIdea.Committee_Rejected == null)
                {
                    existingIdea.Committee_Rejected = new List<int>();
                    existingIdea.CommenterCOMRejected = comment;
                }
                if (!existingIdea.Committee_Rejected.Contains(userId))
                {
                    existingIdea.Committee_Rejected.Add(userId);
                    existingIdea.CommenterCOMRejected = comment;
                }
            }
            else
            {
                return new BadRequestObjectResult("Invalid response");
            }

            await _context.SaveChangesAsync();

            if (existingIdea.Committee_Approved.Count == 3)
            {
                existingIdea.Status = "Approved";
            }
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { Message = response.Equals("approved", StringComparison.OrdinalIgnoreCase) ? "Approved" : "Rejected" });
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            string fileExtension = Path.GetExtension(originalFileName);
            string uniqueFileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
            return uniqueFileName;
        }
    }
}
