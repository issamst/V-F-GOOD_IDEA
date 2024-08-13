using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Area;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Service.Area
{
    public class AreasService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AreasService> _logger;

        public AreasService(AppDbContext context, ILogger<AreasService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Area_M>> GetAllAreas()
        {
            return await _context.Area.Include(a => a.BuildingID).ToListAsync();
        }

        public async Task<Area_M> GetAreaById(int id)
        {
            return await _context.Area.FindAsync(id);
        }

        public async Task<List<Area_M>> GetAreasByPlant(int plantId)
        {
            return await _context.Area.Where(a => a.PlantID == plantId).ToListAsync();
        }

        public async Task<bool> CreateArea(Area_M area)
        {
            try
            {
                area.Date_Create = DateTime.Now;
                _context.Area.Add(area);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating area: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateArea(int id, Area_M area)
        {
            try
            {
                var existingArea = await _context.Area.FirstOrDefaultAsync(x => x.Id == id);
                if (existingArea != null)
                {
                    existingArea.Name_Area = area.Name_Area;
                    existingArea.PlantID = area.PlantID; // Update with the new PlantID
                    existingArea.Date_Update = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating area: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteArea(int id, string commenter)
        {
            try
            {
                var area = await _context.Area.FindAsync(id);
                if (area != null)
                {
                    area.Date_delete = DateTime.Now;
                    area.CommenterDelete = commenter;
                    _context.Area.Update(area);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting area: {ex.Message}");
                return false;
            }
        }
        public async Task<string> ImportExcelAreas(IFormFile file)
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
                        int nameIndex = -1, buildingIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Name") nameIndex = i;
                                    else if (columnName == "Plant") buildingIndex = i;
                                }

                                continue;
                            }

                            var name = reader.GetValue(nameIndex)?.ToString();
                            var building = reader.GetValue(buildingIndex)?.ToString();

                            var plant = await _context.Plant.FirstOrDefaultAsync(p => p.BuildingID == building);

                            if (plant == null)
                            {
                                continue;
                            }

                            // Check if the Area already exists in the database
                            var existingArea = await _context.Area.FirstOrDefaultAsync(a => a.Name_Area == name);

                            if (existingArea != null)
                            {
                                // Update the existing area
                                existingArea.PlantID = plant.Id;
                                existingArea.Date_Update = DateTime.Now;

                                _context.Area.Update(existingArea);
                            }
                            else
                            {
                                // Create a new Area_M object
                                var area = new Area_M
                                {
                                    Name_Area = name,
                                    PlantID = plant.Id,
                                    Date_Create = DateTime.Now,
                                };

                                // Add the new Area_M object to the context
                                await _context.Area.AddAsync(area);
                            }

                            await _context.SaveChangesAsync();
                        }

                        return "Areas uploaded successfully.";
                    }
                }
            }
            else
            {
                return "No file uploaded.";
            }
        }

        public async Task<byte[]> ExportAreas()
        {
            try
            {
                var areas = await _context.Area
                    .Include(a => a.BuildingID)
                    .ToListAsync();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Areas");

                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Plant";

                    for (int i = 0; i < areas.Count; i++)
                    {
                        var area = areas[i];
                        worksheet.Cells[i + 2, 1].Value = area.Name_Area;
                        worksheet.Cells[i + 2, 2].Value = area.BuildingID?.BuildingID;
                    }

                    worksheet.Cells.AutoFitColumns();

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting areas: {ex.Message}");
                throw;
            }
        }

        public async Task<int?> GetAreaIdByName(string name)
        {
            try
            {
                var area = await _context.Area.FirstOrDefaultAsync(a => a.Name_Area == name);

                return area?.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving area ID: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<int>> GetAreaIdsByNames(IEnumerable<string> names)
        {
            try
            {
                var areas = await _context.Area.Where(a => names.Contains(a.Name_Area)).ToListAsync();

                return areas.Select(a => a.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving area IDs: {ex.Message}");
                throw;
            }
        }
    }
}
