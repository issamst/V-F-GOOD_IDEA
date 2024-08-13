using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Models.Area;
using Asp_Net_Good_idea.Service.Area;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace Asp_Net_Good_idea.Controllers.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly MachineService _machineService;
        private readonly ILogger<MachineController> _logger;

        public MachineController(MachineService machineService, ILogger<MachineController> logger)
        {
            _machineService = machineService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine_M>>> GetAllMachines()
        {
            var machines = await _machineService.GetAllMachines();
            return Ok(machines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Machine_M>> GetMachine(int id)
        {
            var machine = await _machineService.GetMachineById(id);
            if (machine == null)
            {
                return NotFound();
            }

            return Ok(machine);
        }

        [HttpPost]
        public async Task<ActionResult<Machine_M>> CreateMachine(Machine_M machine)
        {
            bool isCreated = await _machineService.CreateMachine(machine);
            if (isCreated)
            {
                return CreatedAtAction(nameof(GetMachine), new { id = machine.Id }, machine);
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the machine.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, Machine_M machine)
        {
            bool isUpdated = await _machineService.UpdateMachine(id, machine);
            if (isUpdated)
            {
                return Ok(new { Message = "Machine updated successfully" });
            }
            else
            {
                return NotFound("Machine not found");
            }
        }

        [HttpDelete("deleteMachine/{id}/{commenter}")]
        public async Task<IActionResult> DeleteMachine(int id, string commenter)
        {
            bool isDeleted = await _machineService.DeleteMachine(id, commenter);
            if (isDeleted)
            {
                return Ok(new { Message = "Machine deleted successfully" });
            }
            else
            {
                return NotFound("Machine not found");
            }
        }

        [HttpPost("importMachine")]
        public async Task<IActionResult> ImportExcelMachines(IFormFile file)
        {
            string result = await _machineService.ImportExcelMachines(file);
            if (result == "Machines uploaded successfully.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("exportMachine")]
        public async Task<IActionResult> ExportMachine()
        {
            try
            {
                byte[] fileContents = await _machineService.ExportMachines();
                return File(new MemoryStream(fileContents), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Machines.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting machines: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting machines.");
            }
        }

        [HttpPost("getMachinesByProject")]
        public async Task<ActionResult<IEnumerable<Machine_M>>> GetMachinesByProject(IEnumerable<int> projectId)
        {
            var machines = await _machineService.GetMachinesByProject(projectId);
            if (machines == null || machines.Count() == 0)
            {
                return NotFound();
            }

            return Ok(machines);
        }


    }
}
