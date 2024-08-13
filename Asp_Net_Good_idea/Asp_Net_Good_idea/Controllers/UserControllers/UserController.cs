using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Asp_Net_Good_idea.Entity;
using System.Numerics;
using System.Data;
using System.Drawing;
using Asp_Net_Good_idea.Dto.UserDto;
using Asp_Net_Good_idea.Dto.Jwt;
using Asp_Net_Good_idea.Dto.Email;
using Asp_Net_Good_idea.Models.UserModel;
using Asp_Net_Good_idea.UtilityService.UserService;
using System.IO.Ports;
using Microsoft.Win32;
using Asp_Net_Good_idea.Helpers.UserHelpers;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using ExcelDataReader;
using System.IO;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Asp_Net_Good_idea.Models.Plant;
using Asp_Net_Good_idea.Models.Departement;
namespace Asp_Net_Good_idea.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly ILogger<UserController> _logger;

        private readonly SerialPort _serialPort;
        private readonly UserService _userService;
        private string _detectedBadgeId = "";
        public UserController(AppDbContext appDbContext, UserService userService, ILogger<UserController> logger)
        {
            _authContext = appDbContext;

            _userService = userService;
            _logger = logger;


        }



        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Authenticate authenticate, [FromQuery] bool useBadgeId = false)
        {
            try
            {
                if (authenticate == null)
                {
                    return BadRequest(new { Message = "Authenticate object is null." });
                }
                var scanID = GetDetectedBadgeId();
                authenticate.Badge_id = scanID;
                var result = await _userService.Authenticate(authenticate, useBadgeId);
                return Ok(new TokenApiDto()
                {
                    AccessToken = result.newAccessToken,
                    RefreshToken = result.newRefreshToken,
                    Messager = result.messagepas

                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] Register register)
        {
            try
            {
                // Call the GetScannerPortName method internally to get the detected badge ID
              

                var result = await _userService.Register(register);


                return result;


            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }



        }







        // Method to internally call the HTTP GET endpoint and get the detected badge ID
        private string GetDetectedBadgeId()
        {
            try
            {
                // Create an HTTP context for the internal call
                var httpContext = new DefaultHttpContext();
                httpContext.Request.Method = "GET"; // Set the HTTP method

                // Create a controller context using the HTTP context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                };

                // Create an instance of your controller
                var controller = new UserController(_authContext, _userService, _logger)
                {
                    ControllerContext = controllerContext
                };

                // Call the GetScannerPortName action to get the detected badge ID
                IActionResult actionResult = controller.GetScannerPortName(null); // Pass null or appropriate parameter if needed

                // Extract the detected badge ID from the action result
                if (actionResult is OkObjectResult okObjectResult)
                {
                    dynamic resultData = okObjectResult.Value;
                    string detectedBadgeId = resultData.Message;
                    return detectedBadgeId;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions if needed
                Console.WriteLine($"Error getting detected badge ID: {ex.Message}");
            }

            return null; // Return null if badge ID detection fails
        }



        [HttpGet("process-badge")]
        public IActionResult GetScannerPortName([FromQuery] string Badge_id)
        {
            try
            {
                _detectedBadgeId = ScanBadgeIdFromSerialPort(); // Store the detected badge ID

                if (!String.IsNullOrEmpty(_detectedBadgeId))
                {
                    // Clean the scanned ID
                    string cleanedID = CleanBadgeId(_detectedBadgeId);

                    // Return the cleaned ID to the frontend
                    return Ok(new { Message = cleanedID });
                }
                else
                {
                    return BadRequest(new { Message = "Timeout occurred while reading from the serial port" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Method to scan badge ID from the serial port
        private string ScanBadgeIdFromSerialPort()
        {
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = "COM3"; // Update with your actual COM port
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;

            string scannedID = "";

            try
            {
                serialPort.Open();
                DateTime startTime = DateTime.Now;

                while (String.IsNullOrEmpty(scannedID))
                {
                    if ((DateTime.Now - startTime).TotalSeconds > 5) // Timeout after 10 seconds
                        break;

                    if (serialPort.BytesToRead > 0)
                    {
                        scannedID = serialPort.ReadLine();
                    }
                }
            }
            catch (TimeoutException)
            {
                // Handle timeout exception if needed
            }
            finally
            {
                serialPort.Close();
            }

            return scannedID;
        }

        // Clean up the received badge ID
        private string CleanBadgeId(string rawId)
        {
            // Remove non-alphanumeric characters
            string cleanedId = new string(rawId.Where(c => Char.IsLetterOrDigit(c)).ToArray());
            return cleanedId;
        }



        [HttpPost("AddNew")]
        public async Task<IActionResult> Addnew([FromBody] Register register)
        {
            try
            {
                var result = await _userService.Addnew(register);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }



        [HttpGet("getUserbyid/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Register updateUserDto)
        {
            try
            {
                var result = await _userService.UpdateUser(id, updateUserDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [HttpDelete("deleteUser/{id}/{commenter}")]
        public async Task<IActionResult> DeleteUser(int id, string commenter)
        {
            try
            {
                string result = await _userService.DeleteUser(id, commenter);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        [HttpGet("request/{id}")]
        public async Task<IActionResult> CreateRequest(int id)
        {
            try
            {
                string newPassword = await _userService.CreateRequest(id);
                return Ok(new { Message = newPassword });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }





        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            try
            {
                var result = await _userService.Refresh(tokenApiDto);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }




        [HttpPost("send-reset-email")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmail sendEmail)
        {
            try
            {
                var result = await _userService.SendEmail(sendEmail);
                return Ok(result);
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


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasword(ResetPasswordDto resetPasswordDto)
        {



            try
            {
                var result = await _userService.ResetPassword(resetPasswordDto);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.Message == "User Doesn't Exist")
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        ex.Message
                    });
                }
                else if (ex.Message == "Invalid Reset link")
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        ex.Message
                    });
                }
                else if (ex.Message == "Password Rest Successfully")
                {
                    return BadRequest(new
                    {
                        StatusCode = 200,
                        ex.Message
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Message = "Internal Server Error"
                    });
                }
            }
        }
        [HttpPost("importUser")]
        public async Task<IActionResult> UploadExcelUsers(IFormFile file)
        {
            try
            {
                return await _userService.UploadExcelUsers(file);
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


        [HttpGet("exportUser")]
        public async Task<IActionResult> ExportUser()
        {
            try
            {
                return await _userService.ExportUser();
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

        [HttpGet("getUserIdByTeid/{teid}")]
        public async Task<IActionResult> GetUserIdByTeid(string teid)
        {
            try
            {
                var user = await _authContext.Users.FirstOrDefaultAsync(u => u.TEID == teid);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

                return Ok(user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user by TEID: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the user." });
            }
        }


    }
}