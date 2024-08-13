using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Helpers.UserHelpers;
using Asp_Net_Good_idea.Models.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading.Tasks;
using OfficeOpenXml; // Add this using directive
using ExcelDataReader;
using Microsoft.VisualBasic;
using System.Text;
using Asp_Net_Good_idea.Service.UserService;
using Asp_Net_Good_idea.UtilityService.UserService;

namespace Asp_Net_Good_idea.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly ILogger<UserRoleController> _logger;
        private readonly UserRoleService _userRoleService;

        public UserRoleController(AppDbContext appDbContext, UserRoleService userRoleService, ILogger<UserRoleController> logger)
        {
            _authContext = appDbContext;
            _logger = logger;
            _userRoleService = userRoleService;
        }

        // Get all roles
        [HttpGet]
        public async Task<ActionResult<List<User_Role>>> GetAllRoles()
        {
            try
            {
                var result = await _userRoleService.GetAllRoles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all roles: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        // Get a role by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User_Role>> GetRole(int id)
        {
            try
            {
                var role = await _userRoleService.GetRoleById(id);
                if (role != null)
                {
                    return Ok(role);
                }
                return NotFound("Role not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving role by ID: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }


        //Seed Roles

        [HttpPost("seedRoles")]
        public async Task<IActionResult> SeedRoles()
        {
            var roles = new List<string> { "User", "Admin", "Tema Leader", "Committees",  "Operator" };

            foreach (var roleName in roles)
            {
                if (!await _authContext.User_Role.AnyAsync(r => r.Name_Role == roleName))
                {
                    var role = new User_Role
                    {
                        Name_Role = roleName,
                        Date_Create = DateTime.Now
                    };

                    _authContext.User_Role.Add(role);
                }
            }

            await _authContext.SaveChangesAsync();
            return Ok(new { Message = "Roles seeded successfully" });
        }




        // Add a new role
        [HttpPost]
        public async Task<ActionResult<User_Role>> AddRole([FromBody] User_Role role)
        {
            try
            {
                var success = await _userRoleService.AddRole(role);
                if (success)
                {
                    return Ok(new { Message = "Role added successfully" });
                }
                return BadRequest("Error adding role");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding role: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        // Update a role
        [HttpPut("{id:int}")]
        public async Task<ActionResult<User_Role>> UpdateRole(int id, [FromBody] User_Role role)
        {
            try
            {
                var success = await _userRoleService.UpdateRole(id, role);
                if (success)
                {
                    return Ok(new { Message = "Role updated successfully" });
                }
                return NotFound("Role not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating role: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        // Delete a role
        [HttpDelete("RoleUser/{id}/{commenter}")]
        public async Task<ActionResult<User_Role>> DeleteRole(int id, string commenter)
        {
            try
            {
                var success = await _userRoleService.DeleteRole(id, commenter);
                if (success)
                {
                    return Ok(new { Message = "Role deleted successfully" });
                }
                return NotFound("Role not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting role: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("importRole")]
        public async Task<IActionResult> UploadExcelRoles(IFormFile file)
        {
            try
            {
                return await _userRoleService.UploadExcelUsers(file);

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

        [HttpGet("exportRole")]
        public async Task<IActionResult> ExportRole()
        {
            try
            {
                return await _userRoleService.ExportUser();
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