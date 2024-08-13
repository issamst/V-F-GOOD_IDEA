using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Asp_Net_Good_idea.Models;
using Asp_Net_Good_idea.Context;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Asp_Net_Good_idea.Entity;
using AutoMapper;
using Asp_Net_Good_idea.Dto.Jwt;
using Asp_Net_Good_idea.Dto.UserDto;
using Asp_Net_Good_idea.Dto.Email;
using Asp_Net_Good_idea.Helpers.Email;
using Asp_Net_Good_idea.Helpers.UserHelpers;
using Asp_Net_Good_idea.Models.UserModel;
using Asp_Net_Good_idea.UtilityService.EmailService;
using Asp_Net_Good_idea.Models.EmailModel;
using System.IO.Ports;
using ExcelDataReader;
using OfficeOpenXml;
using NuGet.Protocol.Plugins;
using MimeKit;
using Microsoft.Extensions.DependencyInjection;

namespace Asp_Net_Good_idea.UtilityService.UserService
{
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
            private readonly IEmailService _emailService;
        private readonly AppDbContext _authContext;
        private readonly UserServiceHelpers _userServiceHelpers;
        private readonly ILogger<UserService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly SerialPort _serialPort;
        public UserService(AppDbContext authContext, IConfiguration configuration, IMapper mapper, IServiceScopeFactory serviceScopeFactory, ILogger<UserService> logger, IEmailService emailService, UserServiceHelpers userServiceHelpers)
        {
            _configuration = configuration;
           // _emailService = emailService;
            _authContext = authContext;
            _mapper = mapper;
            _logger = logger;

            _userServiceHelpers = new UserServiceHelpers(authContext); // Initialize UserServiceHelpers
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM3"; // Update with your actual COM port
            _serialPort.BaudRate = 9600;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serviceScopeFactory = serviceScopeFactory;
        }



        public async Task<(string newAccessToken, string newRefreshToken, string messagepas)> Authenticate(Authenticate authenticate, bool useBadgeId = false)
        {
            if (authenticate == null)
            {
                throw new ArgumentNullException(nameof(authenticate), "Authenticate object is null.");
            }
            //if (useBadgeId)
            //{
            //    // Perform authentication using badge ID
            //    var user = await _authContext.Users
            //        .Include(u => u.Name_Role)
            //        .FirstOrDefaultAsync(u => u.Badge_id == authenticate.Badge_id);

            //    if (user == null)
            //    {
            //        throw new Exception("User Badge ID not Found!");
            //    }

            //    // Create JWT, refresh token, and update user data
            //    var jwtData = new CreateJwt
            //    {
            //        FulltName = user.FullName,
            //        Role = user.Name_Role?.Name_Role ?? ""
            //    };

            //    user.Token = _userServiceHelpers.CreateJwt(jwtData);
            //    user.RefreshTokenExpiryTime = DateTime.Now;
            //    await _authContext.SaveChangesAsync();
            //    return (user.Token, _userServiceHelpers.CreateRefreshToken());
            //}
            //else
            //{
            // Perform authentication using TEID and password
            var user = await _authContext.Users
                .Include(u => u.Name_Role)
                .FirstOrDefaultAsync(u => u.TEID == authenticate.TEID);
            var messagepas = "No";

            if (user == null)
            {
                throw new Exception("User Not Found!");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new Exception("User password is null or empty");
            }

            if (string.IsNullOrEmpty(authenticate.Password))
            {
                throw new Exception("Password is null or empty");
            }

            if (!PasswordHasher.VerifyPassword(authenticate.Password, user.Password))
            {
                throw new Exception("Password is Incorrect");
            }

            // Check if RequestPassword is not null and verify the password
            if (!string.IsNullOrEmpty(user.RequestPassword) && PasswordHasher.VerifyPassword(authenticate.Password, user.RequestPassword))
            {
                messagepas = "New Password";
            }

            if (authenticate.Password == user.TEID)
            {
                messagepas = "New Password";
            }

            // Create JWT, refresh token, and update user data
            var jwtData = new CreateJwt
            {
                FulltName = user.FullName,
                Role = user.Name_Role?.Name_Role ?? "",
                Id=user.Id,
                Teid=user.TEID
            };

            user.Token = _userServiceHelpers.CreateJwt(jwtData);
            user.RefreshTokenExpiryTime = DateTime.Now;
            await _authContext.SaveChangesAsync();
            return (user.Token, _userServiceHelpers.CreateRefreshToken(), messagepas);
        }
        public async Task<IActionResult> Register(Register register)
        {
            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }

            //if (await _userServiceHelpers.CheckUserTEIDAsync(register.TEID))
            //{
            //    throw new Exception("TE ID already exists");
            //}

            //if (string.IsNullOrEmpty(register.Email))
            //{
            //    var email_admin = "issam.serbout09@gmail.com";
            //    var emailModel = new EmailModel(email_admin, "New Operateur", NewEmployee.NewBodyEmployee(register.TEID, register.FirstName + " " + register.LastName, register.Phone));
            //    _emailService.SendEmail(emailModel);
            //}
            else
            {
                if (await _userServiceHelpers.CheckUserEMAILAsync(register.Email))
                    throw new Exception("Email already exists");
                var emailModel = new EmailModel(register.Email, "Welcome", NewEmployee.NewBodyEmployee(register.TEID, register.FirstName + " " + register.LastName, register.Phone));
                _emailService.SendEmail(emailModel);
            }

            var newUser = _mapper.Map<User>(register);
            newUser.Password = PasswordHasher.hashPassword(register.Password);
            if (!string.IsNullOrEmpty(register.FirstName) && !string.IsNullOrEmpty(register.LastName))
            {
                newUser.FullName = register.FirstName + " " + register.LastName;
            }

            if (newUser.RoleID != null)
            {

                newUser.RoleID = 1;
            }


            // Check if Status is null or empty
            if (string.IsNullOrEmpty(newUser.Status))
            {
                newUser.Status = "pending";
            }


            newUser.RegisterTime = DateTime.Now;

            await _authContext.Users.AddAsync(newUser);
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                Message = "User Registered!"
            });

        }
        public async Task<IActionResult> Addnew(Register register)
        {
            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (await _userServiceHelpers.CheckUserTEIDAsync(register.TEID))
            {
                throw new Exception("TE ID already exists");
            }

            //if (string.IsNullOrEmpty(register.Email))
            //{
            //    var email_admin = "tykochyco@gmail.com";
            //    var emailModel = new EmailModel(email_admin, "New Operateur", NewEmployee.NewBodyEmployee(register.TEID, register.FirstName + " " + register.LastName, register.Phone));

            //    _emailService.SendEmail(emailModel);
            //}
            //else
            //{
            //    if (await _userServiceHelpers.CheckUserEMAILAsync(register.Email))
            //        throw new Exception("Email already exists");
            //    var emailModel = new EmailModel(register.Email, "Welcome", NewEmployee.NewBodyEmployee(register.TEID, register.FirstName + " " + register.LastName, register.Phone));
            //    _emailService.SendEmail(emailModel);
            //}

            var newUser = _mapper.Map<User>(register);
            newUser.Password = PasswordHasher.hashPassword(register.Password);
            if (!string.IsNullOrEmpty(register.FirstName) && !string.IsNullOrEmpty(register.LastName))
            {
                newUser.FullName = register.FirstName + " " + register.LastName;
            }
            if (newUser.RoleID == null)
            {

                newUser.RoleID = 1;
            }

            // Check if Status is null or empty
            if (string.IsNullOrEmpty(newUser.Status))
            {
                newUser.Status = "pending";
            }


            newUser.RegisterTime = DateTime.Now;

            await _authContext.Users.AddAsync(newUser);
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                Message = "User Registered!"
            });

        }

        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                throw new Exception("Invalid Client Request");

            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;

            var principal = _userServiceHelpers.GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _authContext.Users
                .Include(u => u.Name_Role)
                .FirstOrDefaultAsync(u => u.FullName == username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new Exception("Invalid Request");

            var roleName = user.Name_Role != null ? user.Name_Role.Name_Role : "";
            var jwtData = new CreateJwt
            {
                FulltName = user.FullName,
                Role = roleName
            };
            var newAccessToken = _userServiceHelpers.CreateJwt(jwtData);
            var newRefreshToken = _userServiceHelpers.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(1);
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        public async Task<IActionResult> SendEmail(SendEmail sendEmail)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.TEID == sendEmail.TEID);
            if (user is null)
            {
                throw new Exception("TEID Doesn't Exist");
            }

            if (!string.IsNullOrEmpty(sendEmail.Email))
            {
                var email = await _authContext.Users.FirstOrDefaultAsync(a => a.TEID == sendEmail.TEID && a.Email == sendEmail.Email);
                if (email is null)
                {
                    throw new Exception("Email Doesn't Exist");
                }
            }
            else
            {
                sendEmail.Email = "issam.serbout09@gmail.com";
                user.Request = "pending";
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiryTime = DateTime.Now.AddDays(1);
            string from = _configuration["EmailSettings:From"];
          //  var emailModel = new EmailModel(sendEmail.Email, "Reset Password !!", EmailBodyPassword.EmailStringBody(sendEmail.TEID, sendEmail.Email, emailToken));
           // _emailService.SendEmail(emailModel);

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                StatusCode = 200,
                Message = "Sent!"
            });
        }

        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.TEID == resetPasswordDto.TEID);
            if (user is null)
            {
                throw new Exception("User Doesn't Exist");
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiryTime;

            Console.WriteLine("Token Code: " + tokenCode);
            Console.WriteLine("Input Token: " + resetPasswordDto.EmailToken);
            Console.WriteLine("Token Expiry: " + emailTokenExpiry);

            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                throw new Exception("Invalid Reset link");
            }

            user.Password = PasswordHasher.hashPassword(resetPasswordDto.NewPassword);
            user.Request = "active";

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return new OkObjectResult(new
            {
                StatusCode = 200,
                Message = "Password Reset Successfully"
            });
        }

        public async Task<IActionResult> ResetNewPassword(NewPassword newPassword)
        {
            try
            {
                var user = await _authContext.Users.FirstOrDefaultAsync(a => a.TEID == newPassword.TEID);
                if (user == null)
                {
                    throw new Exception("User Doesn't Exist");
                }

                if (newPassword.Password != newPassword.ConfirmPassword)
                {
                    throw new Exception("Passwords do not match");
                }

                user.Password = PasswordHasher.hashPassword(newPassword.Password);
                user.Request = "active-2";
                user.RequestPassword = "";

                _authContext.Entry(user).State = EntityState.Modified;
                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    StatusCode = 200,
                    Message = "Password Reset Successfully"
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in ResetNewPassword: {ex.Message}");
                if (ex.Message == "Passwords do not match")
                {
                    // Additional logging to debug the issue
                    Console.WriteLine($"CNewPassword: {newPassword.Password}, ConfirmPassword: {newPassword.ConfirmPassword}");
                }
                throw;
            }
        }




        //---------------------->  Updates



        public async Task<IActionResult> UpdateUser(int id, Register updateUserDto)
        {
            try
            {
                var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Map the properties from updateUserDto to the existing user entity
                _mapper.Map(updateUserDto, user);

                // Ensure Date_Update is set manually
                user.Date_Update = DateTime.Now;

                // Save changes to the database
                _authContext.Users.Update(user);
                await _authContext.SaveChangesAsync();

                return new OkObjectResult(new { Message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { ex.Message });
            }
        }
        public async Task<IActionResult> UploadExcelUsers(IFormFile file)
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
                        int teidIndex = -1, firstnameIndex = -1, lastnameIndex = -1, fullnameIndex = -1,
                            phoneIndex = -1, emailIndex = -1, roleNameIndex = -1,
                            titleNameIndex = -1, plantNameIndex = -1, departementNameIndex = -1, SupervisorNameIndex=-1;

                        while (reader.Read())
                        {
                            if (!isHeaderSkipped)
                            {
                                isHeaderSkipped = true;

                                // Iterate through the columns to find the indices by name
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetValue(i)?.ToString();

                                    if (columnName == "TEID") teidIndex = i;
                                    else if (columnName == "First Name") firstnameIndex = i;
                                    else if (columnName == "Last Name") lastnameIndex = i;
                                    else if (columnName == "Full Name") fullnameIndex = i;
                                    else if (columnName == "Phone") phoneIndex = i;
                                    else if (columnName == "Email") emailIndex = i;
                                    else if (columnName == "Role") roleNameIndex = i;
                                    else if (columnName == "Title") titleNameIndex = i;
                                    else if (columnName == "Building") plantNameIndex = i;
                                    else if (columnName == "Department") departementNameIndex = i; else if (columnName == "Supervisor") SupervisorNameIndex = i;
                                }

                                continue;
                            }

                            var teid = reader.GetValue(teidIndex)?.ToString();
                            var Supervisor = reader.GetValue(SupervisorNameIndex)?.ToString();
                            var firstname = reader.GetValue(firstnameIndex)?.ToString();
                            var lastname = reader.GetValue(lastnameIndex)?.ToString();
                            var fullname = reader.GetValue(fullnameIndex)?.ToString() ?? "";
                            var phone = reader.GetValue(phoneIndex)?.ToString();
                            var email = reader.GetValue(emailIndex)?.ToString();
                            var roleName = reader.GetValue(roleNameIndex)?.ToString();
                            var titleName = reader.GetValue(titleNameIndex)?.ToString();
                            var plantName = reader.GetValue(plantNameIndex)?.ToString();
                            var departementName = reader.GetValue(departementNameIndex)?.ToString();
                            var role = await _authContext.User_Role.FirstOrDefaultAsync(r => r.Name_Role == roleName);
                            var title = await _authContext.User_Title.FirstOrDefaultAsync(t => t.Name_Title == titleName);
                            var plant = await _authContext.Plant.FirstOrDefaultAsync(p => p.BuildingID == plantName);
                            var departement = await _authContext.Departement.FirstOrDefaultAsync(d => d.Name_Departement == departementName);


                            if (role == null || title == null || plant == null || departement == null)
                            {
                                continue;
                            }

                            var existingUser = await _authContext.Users.FirstOrDefaultAsync(u => u.TEID == teid);
                            if (existingUser != null)
                            {
                                // Update existing user
                                existingUser.FirstName = firstname;
                                existingUser.LastName = lastname;
                                existingUser.FullName = fullname;
                                existingUser.Phone = phone;
                                existingUser.Email = email;
                                existingUser.RoleID = role.Id;
                                existingUser.PlantID = plant.Id;
                                existingUser.DepartementID = departement.Id;
                                existingUser.TitleID = title.Id;
                                existingUser.Supervisor = Supervisor;

                                _authContext.Users.Update(existingUser);
                                await _authContext.SaveChangesAsync();
                            }
                            if (existingUser == null)
                            {

                                var hashedPassword = PasswordHasher.hashPassword(teid);
                                
                                var user = new User
                                {
                                    TEID = teid,
                                    FirstName = firstname,
                                    LastName = lastname,
                                    FullName = fullname,
                                    Phone = phone,
                                    Password = hashedPassword,
                                    Email = email,
                                    Badge_id = null,
                                    Token = null,
                                    RoleID = role.Id,
                                    PlantID = plant.Id,
                                    DepartementID = departement.Id,
                                    TitleID = title.Id,
                                    Supervisor = Supervisor,
                                    Status = "active",
                                    RefreshToken = null,
                                    ResetPasswordToken = null,
                                    Request = null,
                                    CommenterDelete = null
                                };

                                // Add User object to the context
                                _authContext.Users.Add(user);
                                await _authContext.SaveChangesAsync();
                            }

                            await _authContext.SaveChangesAsync();
                        }

                        return new OkObjectResult("Users uploaded successfully.");
                    }
                }
            }
            else
            {
                return new BadRequestObjectResult("No file uploaded.");
            }
        }
        public async Task<IActionResult> ExportUser()
        {
            try
            {
                // Retrieve all users from the database including related entities
                var users = await _authContext.Users
                    .Include(u => u.Name_Role) // Include the Name_Role navigation property
                    .Include(u => u.Name_Title)
                    .Include(u => u.BuildingID)
                    .Include(u => u.Name_Departement)
                    .ToListAsync();

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {

                    var worksheet = package.Workbook.Worksheets.Add("Users");

                    worksheet.Cells[1, 1].Value = "TEID";
                    worksheet.Cells[1, 2].Value = "First Name";
                    worksheet.Cells[1, 3].Value = "Last Name";
                    worksheet.Cells[1, 4].Value = "Full Name";
                    worksheet.Cells[1, 5].Value = "Phone";
                    worksheet.Cells[1, 6].Value = "Email";
                    worksheet.Cells[1, 7].Value = "Role";
                    worksheet.Cells[1, 8].Value = "Title";
                    worksheet.Cells[1, 9].Value = "Building";
                    worksheet.Cells[1, 10].Value = "Department";
                    worksheet.Cells[1, 11].Value = "Supervisor";
                    for (int i = 0; i < users.Count; i++)
                    {
                        var user = users[i];
                        worksheet.Cells[i + 2, 1].Value = user.TEID;
                        worksheet.Cells[i + 2, 2].Value = user.FirstName;
                        worksheet.Cells[i + 2, 3].Value = user.LastName;
                        worksheet.Cells[i + 2, 4].Value = user.FullName;
                        worksheet.Cells[i + 2, 5].Value = user.Phone;
                        worksheet.Cells[i + 2, 6].Value = user.Email;
                        worksheet.Cells[i + 2, 7].Value = user.Name_Role.Name_Role; // Access the Name_Role property
                        worksheet.Cells[i + 2, 8].Value = user.Name_Title.Name_Title; // Access the Name_Title property
                        worksheet.Cells[i + 2, 9].Value = user.BuildingID.BuildingID; // Access the BuildingID property
                        worksheet.Cells[i + 2, 10].Value = user.Name_Departement.Name_Departement; // Access the Name_Departement property
                        worksheet.Cells[i + 2, 11].Value = user.Supervisor; // Access the Name_Departement property
                    }

                    var stream = new MemoryStream(package.GetAsByteArray());
                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var fileName = "Users.xlsx";

                    // Instead of returning the file directly here, return a tuple with the stream and content information
                    return new FileStreamResult(stream, contentType)
                    {
                        FileDownloadName = fileName
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while exporting users.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<string> CreateRequest(int id)
        {
            var user = await _authContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            string newPassword = GenerateRandomPassword();
            user.Password = PasswordHasher.hashPassword(newPassword);
            user.RequestPassword = PasswordHasher.hashPassword(newPassword);

            user.Request = "active";
            await _authContext.SaveChangesAsync();

            return newPassword;
        }
        public async Task<string> DeleteUser(int id, string commenter)
        {
            try
            {
                var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                user.Date_delete = DateTime.Now;
                user.CommenterDelete = commenter;
                _authContext.Users.Update(user);
                await _authContext.SaveChangesAsync();

                return "User deleted successfully"; // Return a string message
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting user: " + ex.Message);
            }
        }
        public async Task<User> GetUserById(int id)
        {
            try
            {
                var user = await _authContext.Users
                    .Include(u => u.Name_Role)
                    .Include(u => u.Name_Title)
                    .Include(u => u.BuildingID)
                    .Include(u => u.Name_Departement)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user.", ex);
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var users = await _authContext.Users
                                              .Include(u => u.Name_Role)
                                              .Include(u => u.Name_Title)
                                              .Include(u => u.BuildingID)
                                              .Include(u => u.Name_Departement)
                                              .ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                // Log the exception here if necessary
                return new List<User>();
            }
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

