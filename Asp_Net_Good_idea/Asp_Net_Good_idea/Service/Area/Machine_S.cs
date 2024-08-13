using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Area;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Service.Area
{
    public class MachineService
    {
        private readonly AppDbContext _context;

        public MachineService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Machine_M>> GetAllMachines()
        {
            return await _context.Machine.Include(m => m.Project_Name).ToListAsync();
        }

        public async Task<Machine_M> GetMachineById(int id)
        {
            return await _context.Machine.FindAsync(id);
        }

        public async Task<bool> CreateMachine(Machine_M machine)
        {
            machine.Date_Create = DateTime.Now;
            _context.Machine.Add(machine);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMachine(int id, Machine_M machine)
        {
            var existingMachine = await _context.Machine.FirstOrDefaultAsync(x => x.Id == id);
            if (existingMachine != null)
            {
                existingMachine.Machine_Name = machine.Machine_Name;
                existingMachine.Building_ID = machine.Building_ID;
                existingMachine.AreaID = machine.AreaID;
                existingMachine.ProjectID = machine.ProjectID;
                existingMachine.Date_Update = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMachine(int id, string commenter)
        {
            var machine = await _context.Machine.FindAsync(id);
            if (machine != null)
            {
                machine.CommenterDelete = commenter;
                machine.Date_delete = DateTime.Now;
                _context.Machine.Update(machine);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> ImportExcelMachines(IFormFile file)
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
                        int machineNameIndex = -1, buildingIDIndex = -1, areaIDIndex = -1, projectIDIndex = -1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "Machine Name") machineNameIndex = i;
                                    else if (columnName == "Plant") buildingIDIndex = i;
                                    else if (columnName == "Area") areaIDIndex = i;
                                    else if (columnName == "Project ID") projectIDIndex = i;
                                }

                                continue;
                            }
                            var machineName = reader.GetValue(machineNameIndex)?.ToString();
                            var buildingID = reader.GetValue(buildingIDIndex)?.ToString();
                            var areaID = reader.GetValue(areaIDIndex)?.ToString();
                            var projectName = reader.GetValue(projectIDIndex)?.ToString();

                            var project = await _context.Project.FirstOrDefaultAsync(d => d.Project_Name == projectName);

                            if (project == null)
                            {
                                continue;
                            }

                            // Check if the Machine already exists in the database
                            var existingMachine = await _context.Machine.FirstOrDefaultAsync(m => m.Machine_Name == machineName);

                            if (existingMachine != null)
                            {
                                // Update the existing machine
                                existingMachine.Building_ID = buildingID;
                                existingMachine.AreaID = areaID;
                                existingMachine.ProjectID = project.Id;
                                existingMachine.Date_Update = DateTime.Now;

                                _context.Machine.Update(existingMachine);
                            }
                            else
                            {
                                // Create a new Machine_M object
                                var machine = new Machine_M
                                {
                                    Machine_Name = machineName,
                                    Building_ID = buildingID,
                                    AreaID = areaID,
                                    ProjectID = project.Id,
                                    Date_Create = DateTime.Now,
                                };

                                // Add the new Machine_M object to the context
                                await _context.Machine.AddAsync(machine);
                            }

                            await _context.SaveChangesAsync();
                        }

                        return "Machines uploaded successfully.";
                    }
                }
            }
            else
            {
                return "No file uploaded.";
            }
        }

        public async Task<byte[]> ExportMachines()
        {
            var machines = await _context.Machine
                .Include(m => m.Project_Name)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Machines");

                worksheet.Cells[1, 1].Value = "Machine Name";
                worksheet.Cells[1, 2].Value = "Plant";
                worksheet.Cells[1, 3].Value = "Area";
                worksheet.Cells[1, 4].Value = "Project Name";

                for (int i = 0; i < machines.Count; i++)
                {
                    var machine = machines[i];
                    worksheet.Cells[i + 2, 1].Value = machine.Machine_Name;
                    worksheet.Cells[i + 2, 2].Value = machine.Building_ID;
                    worksheet.Cells[i + 2, 3].Value = machine.AreaID;
                    worksheet.Cells[i + 2, 4].Value = machine.Project_Name?.Project_Name;
                }

                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        public async Task<IEnumerable<Machine_M>> GetMachinesByProject(IEnumerable<int> projectId)
        {


            var projects = await _context.Machine
                   .Where(p => projectId.Contains(p.ProjectID))
                   .Include(p => p.Project_Name)
                   .ToListAsync();

            return projects;
        }


    }
}
