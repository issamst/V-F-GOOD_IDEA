using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Dto.Area;
using Asp_Net_Good_idea.Models.Area;
using Asp_Net_Good_idea.Service.Area;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Asp_Net_Good_idea.Controllers.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly AreasService _areasService;
        private readonly ILogger<AreaController> _logger;

        public AreaController(AreasService areasService, ILogger<AreaController> logger)
        {
            _areasService = areasService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area_M>>> GetAllAreas()
        {
            var areas = await _areasService.GetAllAreas();
            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Area_M>> GetArea(int id)
        {
            var area = await _areasService.GetAreaById(id);
            if (area == null)
            {
                return NotFound();
            }

            return Ok(area);
        }

        [HttpGet("plant/{plantId}")]
        public async Task<ActionResult<IEnumerable<Area_M>>> GetAreasByPlant(int plantId)
        {
            try
            {
                var areas = await _areasService.GetAreasByPlant(plantId);
                return Ok(areas);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving areas for plant {plantId}: {ex.Message}");
                return StatusCode(500, $"An error occurred while retrieving areas: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult> CreateArea([FromBody] CreateAreaDto areaDto)
        {
            if (areaDto.PlantIDs == null || areaDto.PlantIDs.Count == 0)
            {
                return BadRequest("PlantIDs are required.");
            }

            foreach (var plantId in areaDto.PlantIDs)
            {
                var area = new Area_M
                {
                    Name_Area = areaDto.Name_Area,
                    PlantID = plantId,
                    Date_Create = DateTime.Now
                };

                bool isCreated = await _areasService.CreateArea(area);

                if (!isCreated)
                {
                    return StatusCode(500, $"An error occurred while creating the area for plant ID {plantId}.");
                }
            }

            return CreatedAtAction(nameof(GetAllAreas), null);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(int id, [FromBody] UpdateAreaDto areaDto)
        {
            if (areaDto.PlantIDs == null || areaDto.PlantIDs.Count == 0)
            {
                return BadRequest("PlantIDs are required.");
            }

            bool anyUpdates = false;

            foreach (var plantId in areaDto.PlantIDs)
            {
                var area = new Area_M
                {
                    Id = id,
                    Name_Area = areaDto.Name_Area,
                    PlantID = plantId,
                    Date_Update = DateTime.Now
                };

                bool isUpdated = await _areasService.UpdateArea(id, area);
                if (isUpdated)
                {
                    anyUpdates = true;
                }
                else
                {
                    return NotFound($"Area with ID {id} not found for plant ID {plantId}.");
                }
            }

            if (anyUpdates)
            {
                return Ok(new { Message = "Area updated successfully" });
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the area.");
            }
        }


        [HttpDelete("deleteArea/{id}/{commenter}")]
        public async Task<IActionResult> DeleteArea(int id, string commenter)
        {
            bool isDeleted = await _areasService.DeleteArea(id, commenter);
            if (isDeleted)
            {
                return Ok(new { Message = "Area deleted successfully" });
            }
            else
            {
                return NotFound("Area not found");
            }
        }

        [HttpPost("importArea")]
        public async Task<IActionResult> ImportExcelAreas(IFormFile file)
        {
            string result = await _areasService.ImportExcelAreas(file);
            if (result == "Areas uploaded successfully.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("exportArea")]
        public async Task<IActionResult> ExportArea()
        {
            try
            {
                byte[] fileContents = await _areasService.ExportAreas();
                return File(new MemoryStream(fileContents), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Areas.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting areas: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting areas.");
            }
        }

        [HttpGet("getIdByName/{name}")]
        public async Task<ActionResult<int>> GetAreaIdByName(string name)
        {
            try
            {
                var areaId = await _areasService.GetAreaIdByName(name);
                if (areaId.HasValue)
                {
                    return Ok(areaId.Value);
                }
                else
                {
                    return NotFound($"Area with name '{name}' not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving area ID for name '{name}': {ex.Message}");
                return StatusCode(500, $"An error occurred while retrieving area ID: {ex.Message}");
            }
        }
        [HttpPost("getIdsByNames")]
        public async Task<ActionResult<IEnumerable<int>>> GetAreaIdsByNames([FromBody] IEnumerable<string> names)
        {
            try
            {
                var areaIds = await _areasService.GetAreaIdsByNames(names);
                if (areaIds.Any())
                {
                    return Ok(areaIds);
                }
                else
                {
                    return NotFound("No areas found with the provided names.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving area IDs: {ex.Message}");
                return StatusCode(500, $"An error occurred while retrieving area IDs: {ex.Message}");
            }
        }
    }
}
