using Asp_Net_Good_idea.Context;
using Asp_Net_Good_idea.Helpers.UserHelpers;
using Asp_Net_Good_idea.Service.Area;
using Asp_Net_Good_idea.Service.Committee;
using Asp_Net_Good_idea.Service.Departement;
using Asp_Net_Good_idea.Service.Ideas;
using Asp_Net_Good_idea.Service.MultimediaService;
using Asp_Net_Good_idea.Service.Plant;

using Asp_Net_Good_idea.Service.SupervisorManager;
using Asp_Net_Good_idea.Service.TeamLeader;
using Asp_Net_Good_idea.Service.UserService;
using Asp_Net_Good_idea.UtilityService.EmailService;
using Asp_Net_Good_idea.UtilityService.MapperService;
using Asp_Net_Good_idea.UtilityService.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MultimediaService>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddScoped<UserTitleService>();
builder.Services.AddScoped<PlantService>();
builder.Services.AddScoped<DepartementService>();
builder.Services.AddScoped<AreasService>();
builder.Services.AddScoped<MachineService>();
builder.Services.AddScoped<IdeasService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TeamLeaderService>();
builder.Services.AddScoped<CommitteeService>();
builder.Services.AddScoped<Supervisor_S>();
builder.Services.AddScoped<UserServiceHelpers>();
builder.Services.AddCors(option =>
option.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
})
            ); ;
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnStr"));
}
            );

builder.Services.AddScoped<IEmailService, EmailService>();






builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecureKeyHereWithAtLeast32BytesLong")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero

    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
