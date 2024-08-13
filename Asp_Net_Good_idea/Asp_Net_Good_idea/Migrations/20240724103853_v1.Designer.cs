﻿// <auto-generated />
using System;
using Asp_Net_Good_idea.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Asp_Net_Good_idea.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240724103853_v1")]
    partial class v1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Area.Area_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name_Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlantID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlantID");

                    b.ToTable("Area", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Area.Machine_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AreaID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Building_ID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Machine_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectID");

                    b.ToTable("Machine", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Area.Project_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AreaID")
                        .HasColumnType("int");

                    b.Property<string>("Building_ID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Project_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AreaID");

                    b.ToTable("Project", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Committee.Committee_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AreaID")
                        .HasColumnType("int");

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartementID")
                        .HasColumnType("int");

                    b.Property<string>("Name_Committee")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlantID")
                        .HasColumnType("int");

                    b.Property<int>("ReplacementID")
                        .HasColumnType("int");

                    b.Property<int>("ResponsibleID")
                        .HasColumnType("int");

                    b.Property<int>("ResponsibleTitleID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AreaID");

                    b.HasIndex("DepartementID");

                    b.HasIndex("PlantID");

                    b.HasIndex("ReplacementID");

                    b.HasIndex("ResponsibleID");

                    b.HasIndex("ResponsibleTitleID");

                    b.ToTable("Committees", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Departement.Departement_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manger_Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name_Departement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlantID")
                        .HasColumnType("int");

                    b.Property<string>("TEID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PlantID");

                    b.ToTable("departement", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.ImageModel.ImageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Impact.Impact_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name_Impact")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Impacts");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Plant.Plant_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuildingID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Delete")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SapBuildingNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Plant");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.TeamLeader.TeamLeader_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AreaID")
                        .HasColumnType("int");

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProjectID")
                        .HasColumnType("int");

                    b.Property<string>("Teamleader_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("shift")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AreaID");

                    b.HasIndex("ProjectID");

                    b.ToTable("TeamLeaders", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.UserModel.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Badge_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartementID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LeaderId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlantID")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Request")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ResetPasswordExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TEID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TitleID")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartementID");

                    b.HasIndex("LeaderId");

                    b.HasIndex("PlantID");

                    b.HasIndex("RoleID");

                    b.HasIndex("TitleID");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.UserModel.User_Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name_Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User_Role");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.UserModel.User_Title", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommenterDelete")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name_Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User_Title");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.idea.Idea_M", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommenterCOMApproved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterCOMRejected")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterDisabled")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterTLApproved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommenterTLRejected")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Committee_Approved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Committee_Rejected")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionSituation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionSolution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileIdeaPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileSituationPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileSolutionPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name_Area")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name_Impact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name_Machine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Project_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Team_Leader_Approved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Team_Leader_Rejected")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ideas", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.supervisorManager.SupervorM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommenterDisabled")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Create")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Update")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_delete")
                        .HasColumnType("datetime2");

                    b.Property<string>("Speciality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("SupervorM", (string)null);
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Area.Area_M", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.Plant.Plant_M", "BuildingID")
                        .WithMany()
                        .HasForeignKey("PlantID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("BuildingID");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Area.Machine_M", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.Area.Project_M", "Project_Name")
                        .WithMany()
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Project_Name");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Area.Project_M", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.Area.Area_M", "Name_Area")
                        .WithMany()
                        .HasForeignKey("AreaID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Name_Area");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Committee.Committee_M", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.Area.Area_M", "N_Area")
                        .WithMany()
                        .HasForeignKey("AreaID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.Departement.Departement_M", "N_Departement")
                        .WithMany()
                        .HasForeignKey("DepartementID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.Plant.Plant_M", "N_Plant")
                        .WithMany()
                        .HasForeignKey("PlantID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.UserModel.User", "Replacement")
                        .WithMany()
                        .HasForeignKey("ReplacementID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.UserModel.User", "Responsible")
                        .WithMany()
                        .HasForeignKey("ResponsibleID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.UserModel.User_Title", "ResponsibleTitle")
                        .WithMany()
                        .HasForeignKey("ResponsibleTitleID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("N_Area");

                    b.Navigation("N_Departement");

                    b.Navigation("N_Plant");

                    b.Navigation("Replacement");

                    b.Navigation("Responsible");

                    b.Navigation("ResponsibleTitle");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.Departement.Departement_M", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.Plant.Plant_M", "BuildingID")
                        .WithMany()
                        .HasForeignKey("PlantID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("BuildingID");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.TeamLeader.TeamLeader_M", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.Area.Area_M", "N_Area")
                        .WithMany()
                        .HasForeignKey("AreaID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.Area.Project_M", "N_Project")
                        .WithMany()
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("N_Area");

                    b.Navigation("N_Project");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.UserModel.User", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.Departement.Departement_M", "Name_Departement")
                        .WithMany()
                        .HasForeignKey("DepartementID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.TeamLeader.TeamLeader_M", "Leader")
                        .WithMany("N_User")
                        .HasForeignKey("LeaderId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Asp_Net_Good_idea.Models.Plant.Plant_M", "BuildingID")
                        .WithMany()
                        .HasForeignKey("PlantID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.UserModel.User_Role", "Name_Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Asp_Net_Good_idea.Models.UserModel.User_Title", "Name_Title")
                        .WithMany()
                        .HasForeignKey("TitleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BuildingID");

                    b.Navigation("Leader");

                    b.Navigation("Name_Departement");

                    b.Navigation("Name_Role");

                    b.Navigation("Name_Title");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.idea.Idea_M", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.UserModel.User", "TEID")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TEID");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.supervisorManager.SupervorM", b =>
                {
                    b.HasOne("Asp_Net_Good_idea.Models.UserModel.User", "TEID")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("TEID");
                });

            modelBuilder.Entity("Asp_Net_Good_idea.Models.TeamLeader.TeamLeader_M", b =>
                {
                    b.Navigation("N_User");
                });
#pragma warning restore 612, 618
        }
    }
}
