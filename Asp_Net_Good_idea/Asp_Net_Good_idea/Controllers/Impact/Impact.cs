using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Impact;
using OfficeOpenXml;
using System.Text;
using Asp_Net_Good_idea.Controllers.UserControllers;
using ExcelDataReader;

namespace Asp_Net_Good_idea.Controllers.Impact
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpactController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger;
        public ImpactController(AppDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Impact_M>>> GetAllImpacts()
        {
            return await _context.Impacts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Impact_M>> GetImpact(int id)
        {
            var impact = await _context.Impacts.FindAsync(id);

            if (impact == null)
            {
                return NotFound();
            }

            return impact;
        }

        [HttpPost]
        public async Task<ActionResult<Impact_M>> CreateImpact(Impact_M impact)
        {
            impact.Date_Create = DateTime.Now; // Set the Date_Create property
            _context.Impacts.Add(impact);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImpact), new { id = impact.Id }, impact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImpact(int id, Impact_M impact)
        {
            var existingImpact = await _context.Impacts.FirstOrDefaultAsync(x => x.Id == id);
            if (existingImpact != null)
            {
                existingImpact.Name_Impact = impact.Name_Impact;
                existingImpact.Description = impact.Description;
                existingImpact.Date_Update = DateTime.Now; // Set the Date_Update property

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Impact updated successfully" });
            }
            return NotFound("Impact not found");
        }

        [HttpDelete("deleteImpact/{id}/{commenter}")]
        public async Task<IActionResult> DeleteImpact(int id, string commenter)
        {
            try
            {
                var impact = await _context.Impacts.FindAsync(id);
                if (impact == null)
                {
                    return NotFound(new { Message = "Impact not found" });
                }

                // Update the impact entity with the commenter information
                impact.CommenterDelete = commenter;
                impact.Date_delete = DateTime.Now;

                // Update the Impact entity in the DbContext
                _context.Impacts.Update(impact);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Impact deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("importImpact")]
        public async Task<IActionResult> UploadExcelImpacts(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        bool isHeaderSkipped = false;
                        int nameIndex = -1, descriptionIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;

                                // Iterate through the columns to find the indices by name
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Impact name") nameIndex = i;
                                    else if (columnName == "Description") descriptionIndex = i;
                                }

                                continue;
                            }

                            var name = reader.GetValue(nameIndex)?.ToString();
                            var description = reader.GetValue(descriptionIndex)?.ToString();

                            // Create Impact object
                            var impact = new Impact_M
                            {
                                Name_Impact = name,
                                Description = description,
                                Date_Create = DateTime.Now

                            };

                            // Add Impact object to the context
                            _context.Impacts.Add(impact);
                            await _context.SaveChangesAsync();
                        }

                        return Ok("Impacts uploaded successfully.");
                    }
                }
            }
            else
            {
                return BadRequest("No file uploaded.");
            }
        }

        [HttpGet("exportImpact")]
        public async Task<IActionResult> ExportImpact()
        {
            try
            {
                // Retrieve all impacts from the database
                var impacts = await _context.Impacts.ToListAsync();

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Impacts");

                    worksheet.Cells[1, 1].Value = "Impact name";
                    worksheet.Cells[1, 2].Value = "Description";

                    for (int i = 0; i < impacts.Count; i++)
                    {
                        var impact = impacts[i];
                        worksheet.Cells[i + 2, 1].Value = impact.Name_Impact;
                        worksheet.Cells[i + 2, 2].Value = impact.Description;
                    }

                    // Auto fit columns
                    worksheet.Cells.AutoFitColumns();

                    // Convert the Excel package to a byte array
                    var excelBytes = package.GetAsByteArray();

                    // Return the Excel file as a FileStreamResult
                    return File(new MemoryStream(excelBytes), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Impacts.xlsx");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error exporting impacts: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting impacts.");
            }
        }

    }
}