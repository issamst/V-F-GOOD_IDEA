using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Models.Area;
using Asp_Net_Good_idea.Service.Area;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Asp_Net_Good_idea.Dto.Project;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Service.Departement;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Asp_Net_Good_idea.Controllers.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project_M>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project_M>> GetProject(int id)
        {
            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project_M>> CreateProject(Project_M projectDto)
        {
            try
            {
                var Project = new Project_M
                {
                    Project_Name = projectDto.Project_Name,
                    Building_ID = projectDto.Building_ID,

                    AreaID = projectDto.AreaID,
                    Date_Create = DateTime.Now
                };

                bool isCreated = await _projectService.CreateProject(Project);

                if (!isCreated)
                {
                    return StatusCode(500, "An error occurred while creating the Project.");
                }

                return CreatedAtAction(nameof(GetAllProjects), new { id = Project.Id }, Project);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating department: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project_M projectDto)
        {
            try
            {
                var Project = new Project_M
                {
                    Project_Name = projectDto.Project_Name,
                    Building_ID = projectDto.Building_ID,
                    AreaID = projectDto.AreaID,
                    Date_Create = DateTime.Now
                };

                bool isUpdated = await _projectService.UpdateProject(id, projectDto);

                if (!isUpdated)
                {
                    return StatusCode(500, "An error occurred while Update the Project.");
                }

                return CreatedAtAction(nameof(GetAllProjects), new { id = Project.Id }, Project);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating department: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpDelete("deleteProject/{id}/{commenter}")]
        public async Task<IActionResult> DeleteProject(int id, string commenter)
        {
            bool isDeleted = await _projectService.DeleteProject(id, commenter);
            if (isDeleted)
            {
                return Ok(new { Message = "Project deleted successfully" });
            }
            else
            {
                return NotFound("Project not found");
            }
        }

        [HttpPost("importProject")]
        public async Task<IActionResult> UploadExcelProjects(IFormFile file)
        {
            string result = await _projectService.ImportExcelProjects(file);
            if (result == "Projects uploaded successfully.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("exportProject")]
        public async Task<IActionResult> ExportProject()
        {
            try
            {
                byte[] fileContents = await _projectService.ExportProjects();
                return File(new MemoryStream(fileContents), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Projects.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting projects: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting projects.");
            }
        }

        [HttpGet("AreaName/{areaId}")]
        public async Task<ActionResult<IEnumerable<Project_M>>> GetProjectsByArea(int areaId)
        {
            try
            {
                var projects = await _projectService.GetProjectsByArea(areaId);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving projects: {ex.Message}");
            }
        }
        [HttpPost("ProjectsByAreas")]
        public async Task<ActionResult<IEnumerable<Project_M>>> GetProjectsByAreas(IEnumerable<int> areaIds)
        {
            try
            {
                var projects = await _projectService.GetProjectsByAreas(areaIds);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving projects: {ex.Message}");
            }
        }

        [HttpPost("ProjectIdByName")]
        public async Task<ActionResult<IEnumerable<int>>> GetProjectIdByName([FromBody] IEnumerable<string> projectName)
        {
            try
            {
                var projectId = await _projectService.GetProjectIdByName(projectName);
                if (projectId != null)
                {
                    return Ok(projectId);
                }
                else
                {
                    return NotFound($"Project with name '{projectName}' not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving project ID by name: {ex.Message}");
            }
        }

    }
}
