using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asp_Net_Good_idea.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Impacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Impact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SapBuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Title", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_Plant_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "departement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Departement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TEID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manger_Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_departement_Plant_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Building_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaID = table.Column<int>(type: "int", nullable: false),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Machine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Machine_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Building_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machine_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamLeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Teamleader_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shift = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamLeaders_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamLeaders_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Badge_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    DepartementID = table.Column<int>(type: "int", nullable: false),
                    TitleID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaderId = table.Column<int>(type: "int", nullable: true),
                    RequestPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_Plant_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_users_TeamLeaders_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "TeamLeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_users_User_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "User_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_User_Title_TitleID",
                        column: x => x.TitleID,
                        principalTable: "User_Title",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_departement_DepartementID",
                        column: x => x.DepartementID,
                        principalTable: "departement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Committees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Committee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlantID = table.Column<int>(type: "int", nullable: false),
                    AreaID = table.Column<int>(type: "int", nullable: false),
                    DepartementID = table.Column<int>(type: "int", nullable: false),
                    ReplacementID = table.Column<int>(type: "int", nullable: false),
                    ResponsibleID = table.Column<int>(type: "int", nullable: false),
                    ResponsibleTitleID = table.Column<int>(type: "int", nullable: false),
                    CommenterDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Committees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Committees_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Committees_Plant_PlantID",
                        column: x => x.PlantID,
                        principalTable: "Plant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Committees_User_Title_ResponsibleTitleID",
                        column: x => x.ResponsibleTitleID,
                        principalTable: "User_Title",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Committees_departement_DepartementID",
                        column: x => x.DepartementID,
                        principalTable: "departement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Committees_users_ReplacementID",
                        column: x => x.ReplacementID,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Committees_users_ResponsibleID",
                        column: x => x.ResponsibleID,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ideas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSituation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_Machine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_Impact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileIdeaPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSituationPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSolutionPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Team_Leader_Approved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterTLApproved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Team_Leader_Rejected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterTLRejected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Committee_Approved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterCOMApproved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Committee_Rejected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterCOMRejected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommenterDisabled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ideas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ideas_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupervorM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Speciality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CommenterDisabled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Update = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_delete = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervorM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupervorM_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Area_PlantID",
                table: "Area",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_AreaID",
                table: "Committees",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_DepartementID",
                table: "Committees",
                column: "DepartementID");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_PlantID",
                table: "Committees",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ReplacementID",
                table: "Committees",
                column: "ReplacementID");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ResponsibleID",
                table: "Committees",
                column: "ResponsibleID");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_ResponsibleTitleID",
                table: "Committees",
                column: "ResponsibleTitleID");

            migrationBuilder.CreateIndex(
                name: "IX_departement_PlantID",
                table: "departement",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_ideas_UserId",
                table: "ideas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Machine_ProjectID",
                table: "Machine",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_AreaID",
                table: "Project",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_SupervorM_UserID",
                table: "SupervorM",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeaders_AreaID",
                table: "TeamLeaders",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLeaders_ProjectID",
                table: "TeamLeaders",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_users_DepartementID",
                table: "users",
                column: "DepartementID");

            migrationBuilder.CreateIndex(
                name: "IX_users_LeaderId",
                table: "users",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_users_PlantID",
                table: "users",
                column: "PlantID");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleID",
                table: "users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_users_TitleID",
                table: "users",
                column: "TitleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "ideas");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Impacts");

            migrationBuilder.DropTable(
                name: "Machine");

            migrationBuilder.DropTable(
                name: "SupervorM");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "TeamLeaders");

            migrationBuilder.DropTable(
                name: "User_Role");

            migrationBuilder.DropTable(
                name: "User_Title");

            migrationBuilder.DropTable(
                name: "departement");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Plant");
        }
    }
}
