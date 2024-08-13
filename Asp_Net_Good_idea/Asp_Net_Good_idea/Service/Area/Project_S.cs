using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Area;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Asp_Net_Good_idea.Service.Area
{
    public class ProjectService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjectService> _logger;
        public ProjectService(AppDbContext context, ILogger<ProjectService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Project_M>> GetAllProjects()
        {
            return await _context.Project.Include(p => p.Name_Area).ToListAsync();
        }

        public async Task<Project_M> GetProjectById(int id)
        {
            return await _context.Project.FindAsync(id);
        }

        public async Task<bool> CreateProject(Project_M project)
        {
            try
            {
                project.Date_Create = DateTime.Now;
                _context.Project.Add(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating Project: {ex.Message}");
                return false;
            }



        }

        public async Task<bool> UpdateProject(int id, Project_M project)
        {
            try
            {
                var existingProject = await _context.Project.FirstOrDefaultAsync(x => x.Id == id);
                if (existingProject != null)
                {
                    existingProject.Project_Name = project.Project_Name;
                    existingProject.Building_ID = project.Building_ID;
                    existingProject.AreaID = project.AreaID;
                    existingProject.Date_Update = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                // Return false if the project was not found
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating Project: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteProject(int id, string commenter)
        {
            var project = await _context.Project.FindAsync(id);
            if (project != null)
            {
                project.Date_delete = DateTime.Now;
                project.CommenterDelete = commenter;
                _context.Project.Update(project);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> ImportExcelProjects(IFormFile file)
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
                        int projectNameIndex = -1, buildingIDIndex = -1, areaIDIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Project Name") projectNameIndex = i;
                                    else if (columnName == "Plant") buildingIDIndex = i;
                                    else if (columnName == "Area Name") areaIDIndex = i;
                                }

                                continue;
                            }

                            var projectName = reader.GetValue(projectNameIndex)?.ToString();
                            var buildingID = reader.GetValue(buildingIDIndex)?.ToString();
                            var areaName = reader.GetValue(areaIDIndex)?.ToString();

                            var area = await _context.Area.FirstOrDefaultAsync(d => d.Name_Area == areaName);

                            if (area == null)
                            {
                                continue;
                            }

                            // Check if the Project already exists in the database
                            var existingProject = await _context.Project.FirstOrDefaultAsync(p => p.Project_Name == projectName);

                            if (existingProject != null)
                            {
                                // Update the existing project
                                existingProject.Building_ID = buildingID;
                                existingProject.AreaID = area.Id;
                                existingProject.Date_Update = DateTime.Now;

                                _context.Project.Update(existingProject);
                            }
                            else
                            {
                                // Create a new Project_M object
                                var project = new Project_M
                                {
                                    Project_Name = projectName,
                                    Building_ID = buildingID,
                                    AreaID = area.Id,
                                    Date_Create = DateTime.Now,
                                };

                                // Add the new Project_M object to the context
                                await _context.Project.AddAsync(project);
                            }

                            await _context.SaveChangesAsync();
                        }

                        return "Projects uploaded successfully.";
                    }
                }
            }
            else
            {
                return "No file uploaded.";
            }
        }

        public async Task<byte[]> ExportProjects()
        {
            var projects = await _context.Project
                .Include(p => p.Name_Area)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Projects");

                worksheet.Cells[1, 1].Value = "Project Name";
                worksheet.Cells[1, 2].Value = "Plant";
                worksheet.Cells[1, 3].Value = "Area Name";

                for (int i = 0; i < projects.Count; i++)
                {
                    var project = projects[i];
                    worksheet.Cells[i + 2, 1].Value = project.Project_Name;
                    worksheet.Cells[i + 2, 2].Value = project.Building_ID;
                    worksheet.Cells[i + 2, 3].Value = project.Name_Area?.Name_Area;
                }

                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        public async Task<IEnumerable<Project_M>> GetProjectsByArea(int areaId)
        {
            return await _context.Project
                .Where(p => p.AreaID == areaId)
                .Include(p => p.Name_Area)
                .ToListAsync();
        }
        public async Task<IEnumerable<Project_M>> GetProjectsByAreas(IEnumerable<int> areaIds)
        {
            try
            {
                var projects = await _context.Project
                    .Where(p => areaIds.Contains(p.AreaID))
                    .Include(p => p.Name_Area)
                    .ToListAsync();

                return projects;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving projects by areas: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<int>> GetProjectIdByName(IEnumerable<string> projectName)
        {
            try
            {
                var project = await _context.Project.Where(a => projectName.Contains(a.Project_Name)).ToListAsync();
                return project.Select(a => a.Id);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving project ID by name: {ex.Message}");
                return null;
            }
        }


    }
}
