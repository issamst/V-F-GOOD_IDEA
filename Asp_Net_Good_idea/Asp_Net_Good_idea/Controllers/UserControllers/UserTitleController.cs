using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Models.UserModel;
using Asp_Net_Good_idea.UtilityService;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ExcelDataReader;
using Microsoft.VisualBasic;
using System.Text;
using Asp_Net_Good_idea.Service.UserService;
using System.Data;
namespace Asp_Net_Good_idea.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTitleController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly ILogger<UserController> _logger;
        private readonly UserTitleService _userTitleService;
        public UserTitleController(AppDbContext appDbContext, UserTitleService userTitleService, ILogger<UserController> logger)
        {
            _authContext = appDbContext;
            _logger = logger;
            _userTitleService = userTitleService;
        }



        [HttpGet]

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User_Title>>> GetAllTitle()
        {
            try
            {
                var result = await _userTitleService.GetAllTitle();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all Titles: {ex.Message}");
                return BadRequest(new { ex.Message });
            }

        }




        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<User_Title>> GetTitle([FromRoute] int id)
        {
            try
            {
                var role = await _userTitleService.GetTitle(id);
                if (role != null)
                {
                    return Ok(role);
                }
                return NotFound("Role not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Titles by ID: {ex.Message}");
                return BadRequest(new { ex.Message });
            }



        }



        [HttpPost]
        public async Task<ActionResult<User_Title>> AddTitle([FromBody] User_Title title)
        {
            try
            {
                var success = await _userTitleService.AddTitle(title);
                if (success)
                {
                    return Ok(new { Message = "Role added successfully" });
                }
                return BadRequest("Error adding role");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding Titles: {ex.Message}");
                return BadRequest(new { ex.Message });
            }

            return Ok(new
            {
                Message = "done !!"
            });

        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<User_Title>> UpdateTitle([FromRoute] int id, [FromBody] User_Title title)
        {
            try
            {
                var success = await _userTitleService.UpdateTitle(id, title);
                if (success)
                {
                    return Ok(new { Message = "Titles  updated successfully" });
                }
                return NotFound("Role not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating role: {ex.Message}");
                return BadRequest(new { ex.Message });
            }


            return NotFound("Title Not found");
        }





        [HttpDelete("TitleUser/{id}/{commenter}")]
        public async Task<ActionResult<User_Title>> DeleteTitle([FromRoute] int id, string commenter)
        {
            try
            {
                var success = await _userTitleService.DeleteTitle(id, commenter);
                if (success)
                {
                    return Ok(new { Message = "Title deleted successfully" });
                }
                return NotFound("Role not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Title: {ex.Message}");
                return BadRequest(new { ex.Message });
            }

        }


        [HttpPost("importTitle")]
        public async Task<IActionResult> UploadExcelTitles(IFormFile file)
        {
            try
            {
                return await _userTitleService.UploadExcelUsers(file);

            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    StatusCode = 200,
                    ex.Message
                });
            }

        }

        // Export titles to Excel
        [HttpGet("exportTitle")]
        public async Task<IActionResult> ExportTitle()
        {
            try
            {
                return await _userTitleService.ExportUser();
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    StatusCode = 200,
                    ex.Message
                });
            }

        }

    }
}