using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.UserModel;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Service.UserService
{
    public class UserRoleService
    {
        private readonly AppDbContext _authContext;
        private readonly ILogger<UserRoleService> _logger;

        public UserRoleService(AppDbContext appDbContext, ILogger<UserRoleService> logger)
        {
            _authContext = appDbContext;
            _logger = logger;
        }

        // Method to get all roles
        public async Task<List<User_Role>> GetAllRoles()
        {
            try
            {
                var userRoles = await _authContext.User_Role.ToListAsync();
                return userRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving roles: {ex.Message}");
                return new List<User_Role>();
            }
        }

        // Method to get a role by ID
        public async Task<User_Role> GetRoleById(int id)
        {
            try
            {
                var role = await _authContext.User_Role.FirstOrDefaultAsync(x => x.Id == id);
                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving role by ID: {ex.Message}");
                return null;
            }
        }

        // Method to add a new role
        public async Task<bool> AddRole(User_Role role)
        {
            try
            {
                role.Date_Create = DateTime.Now;
                await _authContext.User_Role.AddAsync(role);
                await _authContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding role: {ex.Message}");
                return false;
            }
        }

        // Method to update an existing role
        public async Task<bool> UpdateRole(int id, User_Role role)
        {
            try
            {
                var existingRole = await _authContext.User_Role.FirstOrDefaultAsync(x => x.Id == id);
                if (existingRole != null)
                {
                    existingRole.Name_Role = role.Name_Role;
                    existingRole.Date_Update = DateTime.Now;
                    await _authContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating role: {ex.Message}");
                return false;
            }
        }

        // Method to delete a role
        public async Task<bool> DeleteRole(int id, string commenter)
        {
            try
            {
                var existingRole = await _authContext.User_Role.FirstOrDefaultAsync(x => x.Id == id);
                if (existingRole != null)
                {
                    existingRole.CommenterDelete = commenter;
                    existingRole.Date_delete = DateTime.Now;
                    _authContext.User_Role.Update(existingRole);
                    await _authContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting role: {ex.Message}");
                return false;
            }
        }

        public async Task<IActionResult> ExportUser()
        {
            try
            {
                // Retrieve all roles from the database
                var roles = await _authContext.User_Role.ToListAsync();

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Roles");



                    worksheet.Cells[1, 1].Value = "Role Name";
                    worksheet.Cells[1, 2].Value = "Date Created";
                    worksheet.Cells[1, 3].Value = "Last Updated";
                    worksheet.Cells[1, 4].Value = "Date Deleted";
                    worksheet.Cells[1, 5].Value = "Deleted By";

                    // Adding role data
                    for (int i = 0; i < roles.Count; i++)
                    {
                        var role = roles[i];

                        worksheet.Cells[i + 2, 1].Value = role.Name_Role;
                        worksheet.Cells[i + 2, 2].Value = role.Date_Create;
                        worksheet.Cells[i + 2, 3].Value = role.Date_Update;
                        worksheet.Cells[i + 2, 4].Value = role.Date_delete;
                        worksheet.Cells[i + 2, 5].Value = role.CommenterDelete;
                    }


                    worksheet.Cells.AutoFitColumns();

                    // Convert the Excel package to a byte array
                    var excelBytes = package.GetAsByteArray();

                    var stream = new MemoryStream(package.GetAsByteArray());
                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var fileName = "Roles.xlsx";

                    // Instead of returning the file directly here, return a tuple with the stream and content information
                    return new FileStreamResult(stream, contentType)
                    {
                        FileDownloadName = fileName
                    };

                    // Auto fit columns

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while exporting users.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }



        public async Task<IActionResult> UploadExcelUsers(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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
                        int nameRoleIndex = -1, dateCreateIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;

                                // Iterate through the columns to find the indices by name
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Role Name") nameRoleIndex = i;

                                }

                                continue;
                            }

                            var roleName = reader.GetValue(nameRoleIndex)?.ToString();


                            // Create User_Role object
                            var role = new User_Role
                            {
                                Name_Role = roleName,
                                Date_Create = DateTime.Now

                            };

                            // Add User_Role object to the context
                            _authContext.User_Role.Add(role);
                            await _authContext.SaveChangesAsync();
                        }

                        return new OkObjectResult("Roles uploaded successfully.");
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