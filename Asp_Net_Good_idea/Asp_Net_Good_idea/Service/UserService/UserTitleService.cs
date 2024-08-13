using System;
using System.Text;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.UserModel;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Asp_Net_Good_idea.Service.UserService
{
    public class UserTitleService
    {

        private readonly AppDbContext _authContext;
        private readonly ILogger<UserTitleService> _logger;

        public UserTitleService(AppDbContext appDbContext, ILogger<UserTitleService> logger)
        {
            _authContext = appDbContext;
            _logger = logger;
        }
        public async Task<List<User_Title>> GetAllTitle()
        {
            try
            {
                var titles = await _authContext.User_Title.ToListAsync();
                return titles;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Titles: {ex.Message}");
                return new List<User_Title>();
            }
        }


        // Method to get a Title by ID
        public async Task<User_Title> GetTitle(int id)
        {
            try
            {
                var title = await _authContext.User_Title.FirstOrDefaultAsync(x => x.Id == id);
                return title;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Title by ID: {ex.Message}");
                return null;
            }
        }

        // Method to add a new Title
        public async Task<bool> AddTitle(User_Title title)
        {
            try
            {
                title.Date_Create = DateTime.Now;
                await _authContext.User_Title.AddAsync(title);
                await _authContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding Title: {ex.Message}");
                return false;
            }
        }

        // Method to update an existing Title
        public async Task<bool> UpdateTitle(int id, User_Title title)
        {
            try
            {
                var existingtitle = await _authContext.User_Title.FirstOrDefaultAsync(x => x.Id == id);
                if (existingtitle != null)
                {
                    existingtitle.Name_Title = title.Name_Title;
                    existingtitle.Date_Update = DateTime.Now;
                    await _authContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Title: {ex.Message}");
                return false;
            }
        }

        // Method to delete a Title
        public async Task<bool> DeleteTitle(int id, string commenter)
        {
            try
            {
                var existingTitle = await _authContext.User_Title.FirstOrDefaultAsync(x => x.Id == id);
                if (existingTitle != null)
                {
                    existingTitle.CommenterDelete = commenter;
                    existingTitle.Date_delete = DateTime.Now;
                    _authContext.User_Title.Update(existingTitle); // Update User_Title entity
                    await _authContext.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Title: {ex.Message}");
                return false;
            }
        }

        public async Task<IActionResult> ExportUser()
        {
            try
            {
                // Retrieve all titles from the database
                var titles = await _authContext.User_Title.ToListAsync();

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Titles");

                    // Adding headers
                    worksheet.Cells[1, 1].Value = "Title Name";



                    for (int i = 0; i < titles.Count; i++)
                    {
                        var title = titles[i];
                        worksheet.Cells[i + 2, 1].Value = title.Name_Title;
                    }

                    // Auto fit columns
                    worksheet.Cells.AutoFitColumns();

                    // Convert the Excel package to a byte array
                    var excelBytes = package.GetAsByteArray();

                    // Return the Excel file as a FileStreamResult



                    var stream = new MemoryStream(package.GetAsByteArray());
                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var fileName = "Titles.xlsx";

                    // Instead of returning the file directly here, return a tuple with the stream and content information
                    return new FileStreamResult(stream, contentType)
                    {
                        FileDownloadName = fileName
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while exporting Titles.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }



        public async Task<IActionResult> UploadExcelUsers(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

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
                        int nameTitleIndex = -1, dateCreateIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;

                                // Iterate through the columns to find the indices by name
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Title Name") nameTitleIndex = i;

                                }

                                continue;
                            }

                            var titleName = reader.GetValue(nameTitleIndex)?.ToString();


                            // Create User_Title object
                            var title = new User_Title
                            {
                                Name_Title = titleName,
                                Date_Create = DateTime.Now,

                            };

                            // Add User_Title object to the context
                            _authContext.User_Title.Add(title);
                            await _authContext.SaveChangesAsync();
                        }

                        return new OkObjectResult("Titles uploaded successfully.");
                    }
                }
            }
            else
            {
                return new BadRequestObjectResult("No file uploaded.");
            }
        }

    }
}