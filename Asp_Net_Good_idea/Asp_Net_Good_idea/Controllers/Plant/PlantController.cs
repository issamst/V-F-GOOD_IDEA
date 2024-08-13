using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Models.Plant;
using Microsoft.Extensions.Logging;
using Asp_Net_Good_idea.Service.Plant;

namespace Asp_Net_Good_idea.Controllers.Plant
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly PlantService _plantService;
        private readonly ILogger<PlantController> _logger;

        public PlantController(PlantService plantService, ILogger<PlantController> logger)
        {
            _plantService = plantService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant_M>>> GetAllPlants()
        {
            try
            {
                var result = await _plantService.GetAllPlants();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all plants: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plant_M>> GetPlant(int id)
        {
            try
            {
                var plant = await _plantService.GetPlantById(id);
                if (plant != null)
                {
                    return Ok(plant);
                }
                return NotFound("Plant not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving plant by ID: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlant(Plant_M plant)
        {
            try
            {
                var success = await _plantService.CreatePlant(plant);
                if (success)
                {
                    return Ok(new { Message = "Plant created successfully" });
                }
                return BadRequest("Error creating plant");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating plant: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlant(int id, Plant_M plant)
        {
            try
            {
                var success = await _plantService.UpdatePlant(id, plant);
                if (success)
                {
                    return Ok(new { Message = "Plant updated successfully" });
                }
                return NotFound("Plant not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating plant: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("deletePlant/{id}/{commenter}")]
        public async Task<IActionResult> DeletePlant(int id, string commenter)
        {
            try
            {
                var success = await _plantService.DeletePlant(id, commenter);
                if (success)
                {
                    return Ok(new { Message = "Plant deleted successfully" });
                }
                return NotFound("Plant not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting plant: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("importPlants")]
        public async Task<IActionResult> ImportExcelPlants(IFormFile file)
        {
            try
            {
                var result = await _plantService.ImportExcelPlants(file);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error importing plants: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("exportPlants")]
        public async Task<IActionResult> ExportPlants()
        {
            try
            {
                var result = await _plantService.ExportPlants();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting plants: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting plants.");
            }
        }

        [HttpGet("getPlantIdByBuildingId/{buildingId}")]
        public async Task<ActionResult<int>> GetPlantIdByBuildingId(string buildingId)
        {
            try
            {
                var plantId = await _plantService.GetPlantIdByBuildingId(buildingId);
                if (plantId.HasValue)
                {
                    return Ok(plantId.Value);
                }
                return NotFound("Plant not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving plant ID by BuildingID: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }
        [HttpGet("getPlantIdByBuilding/{id}")]
        public async Task<ActionResult<string>> GetPlantIdByBuilding(int id)
        {
            try
            {
                var plantIdString = await _plantService.GetPlantIdByBuilding(id);
                if (plantIdString != null)
                {
                    return Ok(plantIdString); // Returning the BuildingID string
                }
                return NotFound("Plant not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving plant ID by BuildingID: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }


    }
}
