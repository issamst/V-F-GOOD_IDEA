using Asp_Net_Good_idea.Dto.Departement;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.supervisorManager;
using Asp_Net_Good_idea.Service.SupervisorManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asp_Net_Good_idea.Controllers.SupervisorManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorController : ControllerBase
    {
        private readonly Supervisor_S _supervisorService;
        private readonly ILogger<SupervisorController> _logger;

        public SupervisorController(Supervisor_S supervisorService, ILogger<SupervisorController> logger)
        {
            _supervisorService = supervisorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupervorM>>> GetAllSupervisors()
        {
            try
            {
                var supervisors = await _supervisorService.GetAllSupervisors();
                return Ok(supervisors);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving supervisors: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupervorM>> GetSupervisor(int id)
        {
            try
            {
                var supervisor = await _supervisorService.GetSupervisorById(id);
                if (supervisor == null)
                {
                    return NotFound("Supervisor not found");
                }
                return Ok(supervisor);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving supervisor: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<ActionResult<SupervorM>> CreateSupervisor([FromBody] SupervorM supervisor)
        {
            try
            {
                bool isCreated = await _supervisorService.CreateSupervisor(supervisor);

                if (!isCreated)
                {
                    return StatusCode(500, "An error occurred while creating the supervisor.");
                }

                return CreatedAtAction(nameof(GetAllSupervisors), new { id = supervisor.Id }, supervisor);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating supervisor: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupervisor(int id, [FromBody] SupervorM supervisor)
        {
            try
            {
                var existingSupervisor = await _supervisorService.GetSupervisorById(id);
                if (existingSupervisor == null)
                {
                    return NotFound("Supervisor not found");
                }

                bool success = await _supervisorService.UpdateSupervisor(id, supervisor);
                if (!success)
                {
                    return StatusCode(500, "An error occurred while updating the supervisor.");
                }

                return Ok(new { Message = "Supervisor updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating supervisor: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}/{commenter}")]
        public async Task<IActionResult> DeleteSupervisor(int id, string commenter)
        {
            try
            {
                bool success = await _supervisorService.DeleteSupervisor(id, commenter);
                if (!success)
                {
                    return NotFound("Supervisor not found");
                }
                return Ok(new { Message = "Supervisor deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting supervisor: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
