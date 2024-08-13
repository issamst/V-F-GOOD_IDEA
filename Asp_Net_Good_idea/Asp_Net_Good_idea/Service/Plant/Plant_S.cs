using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Plant;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Service.Plant
{
    public class PlantService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlantService> _logger;

        public PlantService(AppDbContext context, ILogger<PlantService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Plant_M>> GetAllPlants()
        {
            return await _context.Plant.ToListAsync();
        }

        public async Task<Plant_M> GetPlantById(int id)
        {
            return await _context.Plant.FindAsync(id);
        }

        public async Task<bool> CreatePlant(Plant_M plant)
        {
            try
            {
                plant.Date_Create = DateTime.Now;
                _context.Plant.Add(plant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating plant: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePlant(int id, Plant_M plant)
        {
            try
            {
                var existingPlant = await _context.Plant.FirstOrDefaultAsync(x => x.Id == id);
                if (existingPlant != null)
                {
                    existingPlant.BuildingID = plant.BuildingID;
                    existingPlant.SapBuildingNumber = plant.SapBuildingNumber;
                    existingPlant.BU = plant.BU;
                    existingPlant.Location = plant.Location;
                    existingPlant.Date_Update = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating plant: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeletePlant(int id, string commenter)
        {
            try
            {
                var plant = await _context.Plant.FindAsync(id);
                if (plant != null)
                {
                    plant.Date_Delete = DateTime.Now;
                    plant.CommenterDelete = commenter;
                    _context.Plant.Update(plant);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting plant: {ex.Message}");
                return false;
            }
        }
        public async Task<IActionResult> ImportExcelPlants(IFormFile file)
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
                        int buildingIDIndex = -1, sapBuildingNumberIndex = -1, buIndex = -1,
                            locationIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;

                                // Iterate through the columns to find the indices by name
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Building ID") buildingIDIndex = i;
                                    else if (columnName == "Sap Building Number") sapBuildingNumberIndex = i;
                                    else if (columnName == "BU") buIndex = i;
                                    else if (columnName == "Location") locationIndex = i;
                                }

                                continue;
                            }

                            var buildingID = reader.GetValue(buildingIDIndex)?.ToString();
                            var sapBuildingNumber = reader.GetValue(sapBuildingNumberIndex)?.ToString();
                            var bu = reader.GetValue(buIndex)?.ToString();
                            var location = reader.GetValue(locationIndex)?.ToString();

                            // Check if the Plant already exists in the database
                            var existingPlant = await _context.Plant.FirstOrDefaultAsync(p => p.BuildingID == buildingID);

                            if (existingPlant != null)
                            {
                                // Update the existing plant
                                existingPlant.SapBuildingNumber = sapBuildingNumber;
                                existingPlant.BU = bu;
                                existingPlant.Location = location;
                                existingPlant.Date_Update = DateTime.Now;

                                _context.Plant.Update(existingPlant);
                            }
                            else
                            {
                                // Create a new Plant object
                                var plant = new Plant_M
                                {
                                    BuildingID = buildingID,
                                    SapBuildingNumber = sapBuildingNumber,
                                    BU = bu,
                                    Location = location,
                                    Date_Create = DateTime.Now,
                                };

                                // Add the new Plant object to the context
                                await _context.Plant.AddAsync(plant);
                            }

                            await _context.SaveChangesAsync();
                        }

                        return new OkObjectResult("Plants uploaded successfully.");
                    }
                }
            }
            else
            {
                return new BadRequestObjectResult("No file uploaded.");
            }
        }

        public async Task<IActionResult> ExportPlants()
        {
            try
            {
                // Retrieve all plants from the database
                var plants = await _context.Plant.ToListAsync();

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Plants");

                    // Adding headers
                    worksheet.Cells[1, 1].Value = "Building ID";
                    worksheet.Cells[1, 2].Value = "Sap Building Number";
                    worksheet.Cells[1, 3].Value = "BU";
                    worksheet.Cells[1, 4].Value = "Location";

                    // Adding plant data
                    for (int i = 0; i < plants.Count; i++)
                    {
                        var plant = plants[i];
                        worksheet.Cells[i + 2, 1].Value = plant.BuildingID;
                        worksheet.Cells[i + 2, 2].Value = plant.SapBuildingNumber;
                        worksheet.Cells[i + 2, 3].Value = plant.BU;
                        worksheet.Cells[i + 2, 4].Value = plant.Location;
                    }

                    // Auto fit columns
                    worksheet.Cells.AutoFitColumns();

                    // Convert the Excel package to a byte array
                    var excelBytes = package.GetAsByteArray();

                    // Return the Excel file as a FileStreamResult
                    return new FileStreamResult(new MemoryStream(excelBytes), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Plants.xlsx"
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error exporting plants: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<int?> GetPlantIdByBuildingId(string buildingId)
        {
            var plant = await _context.Plant.FirstOrDefaultAsync(p => p.BuildingID == buildingId);
            return plant?.Id;
        }
        public async Task<string?> GetPlantIdByBuilding(int id)
        {
            var plant = await _context.Plant.FirstOrDefaultAsync(p => p.Id == id);
            return plant?.BuildingID;
        }
    }
}
