using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Departement;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Service.Departement
{
    public class DepartementService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DepartementService> _logger;

        public DepartementService(AppDbContext context, ILogger<DepartementService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Departement_M>> GetAllDepartements()
        {
            return await _context.Departement.Include(d => d.BuildingID).ToListAsync();
        }

        public async Task<Departement_M> GetDepartementById(int id)
        {
            return await _context.Departement.FindAsync(id);
        }

        public async Task<bool> CreateDepartement(Departement_M departement)
        {
            try
            {
                departement.Date_Create = DateTime.Now;
                _context.Departement.Add(departement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating department: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDepartement(int id, Departement_M departement)
        {
            try
            {
                var existingDepartement = await _context.Departement.FirstOrDefaultAsync(x => x.Id == id);
                if (existingDepartement != null)
                {
                    existingDepartement.Name_Departement = departement.Name_Departement;
                    existingDepartement.TEID = departement.TEID;
                    existingDepartement.Manger_Email = departement.Manger_Email;
                    existingDepartement.PlantID = departement.PlantID;
                    existingDepartement.FullName = departement.FullName;
                    existingDepartement.Date_Update = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating department: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteDepartement(int id, string commenter)
        {
            try
            {
                var departement = await _context.Departement.FindAsync(id);
                if (departement != null)
                {
                    departement.Date_delete = DateTime.Now;
                    departement.CommenterDelete = commenter;
                    _context.Departement.Update(departement);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting department: {ex.Message}");
                return false;
            }
        }

        public async Task<IActionResult> ImportExcelDepartements(IFormFile file)
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
                        int nameIndex = -1, teidIndex = -1, managerEmailIndex = -1, plantIdIndex = -1, fullNameIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Department name") nameIndex = i;
                                    else if (columnName == "TE ID") teidIndex = i;
                                    else if (columnName == "Full Name") fullNameIndex = i;
                                    else if (columnName == "Manager Email") managerEmailIndex = i;
                                    else if (columnName == "Plant") plantIdIndex = i;
                                }

                                continue;
                            }

                            var name = reader.GetValue(nameIndex)?.ToString();
                            var teid = reader.GetValue(teidIndex)?.ToString();
                            var fullname = reader.GetValue(fullNameIndex)?.ToString();
                            var managerEmail = reader.GetValue(managerEmailIndex)?.ToString();
                            var plantName = reader.GetValue(plantIdIndex)?.ToString();
                            var plant = await _context.Plant.FirstOrDefaultAsync(d => d.BuildingID == plantName);

                            if (plant == null)
                            {
                                continue;
                            }

                            // Check if the department already exists in the database
                            var existingDepartement = await _context.Departement.FirstOrDefaultAsync(d => d.Name_Departement == name);

                            if (existingDepartement != null)
                            {
                                // Update the existing department
                                existingDepartement.TEID = teid;
                                existingDepartement.FullName = fullname;
                                existingDepartement.Manger_Email = managerEmail;
                                existingDepartement.PlantID = plant.Id;
                                existingDepartement.Date_Update = DateTime.Now;

                                _context.Departement.Update(existingDepartement);
                            }
                            else
                            {
                                // Create a new Departement_M object
                                var departement = new Departement_M
                                {
                                    Name_Departement = name,
                                    TEID = teid,
                                    FullName= fullname,
                                    Manger_Email = managerEmail,
                                    PlantID = plant.Id,
                                    Date_Create = DateTime.Now,
                                };

                                // Add the new Departement_M object to the context
                                await _context.Departement.AddAsync(departement);
                            }

                            await _context.SaveChangesAsync();
                        }

                        return new OkObjectResult("Departments uploaded successfully.");
                    }
                }
            }
            else
            {
                return new BadRequestObjectResult("No file uploaded.");
            }
        }

        public async Task<IActionResult> ExportDepartements()
        {
            try
            {
                var departments = await _context.Departement
                    .Include(d => d.BuildingID)
                    .ToListAsync();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Departments");

                    worksheet.Cells[1, 1].Value = "Department name";
                    worksheet.Cells[1, 2].Value = "TE ID";
                    worksheet.Cells[1, 3].Value = "Full Name";
                    worksheet.Cells[1, 4].Value = "Manager Email";
                    worksheet.Cells[1, 5].Value = "Plant";

                    for (int i = 0; i < departments.Count; i++)
                    {
                        var department = departments[i];
                        worksheet.Cells[i + 2, 1].Value = department.Name_Departement;
                        worksheet.Cells[i + 2, 2].Value = department.TEID;
                        worksheet.Cells[i + 2, 3].Value = department.FullName;
                        worksheet.Cells[i + 2, 4].Value = department.Manger_Email;
                        worksheet.Cells[i + 2, 5].Value = department.BuildingID?.BuildingID;
                    }

                    worksheet.Cells.AutoFitColumns();

                    var excelBytes = package.GetAsByteArray();

                    return new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Departments.xlsx"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting departments: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
        public async Task<List<Departement_M>> GetDepartementsByPlantId(int plantId)
        {
            try
            {
                return await _context.Departement
                                     .Include(d => d.BuildingID)
                                     .Where(d => d.PlantID == plantId)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving departments for PlantID {plantId}: {ex.Message}");
                return new List<Departement_M>();
            }
        }

    }
}
