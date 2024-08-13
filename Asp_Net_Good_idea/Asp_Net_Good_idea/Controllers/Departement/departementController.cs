using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.Departement;
using ExcelDataReader;
using OfficeOpenXml;
using System.Text;
using Microsoft.Extensions.Logging;
using Asp_Net_Good_idea.Service.Departement;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

using Asp_Net_Good_idea.Models.Area;
using Asp_Net_Good_idea.Models.Dto;
using Asp_Net_Good_idea.Service.Area;
using Asp_Net_Good_idea.Models.Plant;
using Asp_Net_Good_idea.Dto.Departement;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Asp_Net_Good_idea.Controllers.Departement
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartementController : ControllerBase
    {
        private readonly DepartementService _departementService;
        private readonly ILogger<DepartementController> _logger;

        public DepartementController(DepartementService departementService, ILogger<DepartementController> logger)
        {
            _departementService = departementService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departement_M>>> GetAllDepartements()
        {
            try
            {
                var departements = await _departementService.GetAllDepartements();
                return Ok(departements);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving departments: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Departement_M>> GetDepartement(int id)
        {
            try
            {
                var departement = await _departementService.GetDepartementById(id);
                if (departement == null)
                {
                    return NotFound("Departement not found");
                }
                return Ok(departement);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving department: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Departement_M>> CreateDepartement([FromBody] CreateDepartementDto createDto)
        {
            try
            {
                var departement = new Departement_M
                {
                    Name_Departement = createDto.name_Departement,
                    Manger_Email = createDto.manger_Email,
                    TEID = createDto.teid,
                    FullName = createDto.FullName,
                    PlantID = createDto.plantID,
                    Date_Create = DateTime.Now
                };

                bool isCreated = await _departementService.CreateDepartement(departement);

                if (!isCreated)
                {
                    return StatusCode(500, "An error occurred while creating the departement.");
                }

                return CreatedAtAction(nameof(GetAllDepartements), new { id = departement.Id }, departement);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating department: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartement(int id, [FromBody] UpdateDepartementDto updateDto)
        {
            try
            {
                var departement = await _departementService.GetDepartementById(id);
                if (departement == null)
                {
                    return NotFound("Departement not found");
                }

                departement.Name_Departement = updateDto.name_Departement;
                departement.Manger_Email = updateDto.manger_Email;
                departement.TEID = updateDto.teid;
                departement.FullName = updateDto.fullName;
                departement.PlantID = updateDto.plantID;

                var success = await _departementService.UpdateDepartement(id, departement);
                if (!success)
                {
                    return StatusCode(500, "An error occurred while updating the departement.");
                }

                return Ok(new { Message = "Departement updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating department: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }





        [HttpDelete("deleteDepartement/{id}/{commenter}")]
        public async Task<IActionResult> DeleteDepartement(int id, string commenter)
        {
            try
            {
                var success = await _departementService.DeleteDepartement(id, commenter);
                if (!success)
                {
                    return NotFound("Departement not found");
                }
                return Ok(new { Message = "Departement deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting department: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("importDepartement")]
        public async Task<IActionResult> ImportDepartement(IFormFile file)
        {
            try
            {
                var result = await _departementService.ImportExcelDepartements(file);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error importing departments: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("exportDepartement")]
        public async Task<IActionResult> ExportDepartement()
        {
            try
            {
                var result = await _departementService.ExportDepartements();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting departments: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting departments.");
            }
        }
        [HttpGet("byPlant/{plantId}")]
        public async Task<ActionResult<IEnumerable<Departement_M>>> GetDepartementsByPlantId(int plantId)
        {
            try
            {
                var departements = await _departementService.GetDepartementsByPlantId(plantId);
                return Ok(departements);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving departments for PlantID {plantId}: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
