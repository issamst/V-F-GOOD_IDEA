using Asp_Net_Good_idea.Models.Area;
using Asp_Net_Good_idea.Models.Committee;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.idea;
using Asp_Net_Good_idea.Models.ImageModel;
using Asp_Net_Good_idea.Models.Impact;
using Asp_Net_Good_idea.Models.Plant;
using Asp_Net_Good_idea.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Asp_Net_Good_idea.Models.TeamLeader;
using Asp_Net_Good_idea.Models.supervisorManager;


namespace Asp_Net_Good_idea.Context
{
    public class AppDbContext : DbContext
    {


        /*ctor*/
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<User_Title> User_Title { get; set; }
        public DbSet<User_Role> User_Role { get; set; }

        public DbSet<ImageModel> Images { get; set; }
        public DbSet<Departement_M> Departement { get; set; }
        public DbSet<Plant_M> Plant { get; set; }
        public DbSet<Idea_M> Idea { get; set; }
        public DbSet<Area_M> Area { get; set; }
        public DbSet<Machine_M> Machine { get; set; }
        public DbSet<Project_M> Project { get; set; }
        public DbSet<Impact_M> Impacts { get; set; }
        public DbSet<Committee_M> Committees { get; set; }
        public DbSet<TeamLeader_M> TeamLeaders { get; set; }
        public DbSet<SupervorM> SupervorM { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasOne(u => u.Name_Title)
                .WithMany()
                .HasForeignKey(u => u.TitleID);

            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasOne(u => u.Name_Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID);
           


            // Relationship between Committee and Area
            modelBuilder.Entity<Committee_M>()
                .ToTable("Committees")
                .HasOne(c => c.N_Area) // Committee has one Area
                .WithMany() // Area has many Committees
                .HasForeignKey(c => c.AreaID) // Foreign key property
                .OnDelete(DeleteBehavior.NoAction); // No action on deletion

            // relationship between Committee and Plant
            modelBuilder.Entity<Committee_M>()
                .ToTable("Committees")
                .HasOne(c => c.N_Plant)
                .WithMany()
                .HasForeignKey(c => c.PlantID)
                .OnDelete(DeleteBehavior.NoAction);

            // relationship between Committee and Departement
            modelBuilder.Entity<Committee_M>()
               .ToTable("Committees")
               .HasOne(c => c.N_Departement)
               .WithMany()
               .HasForeignKey(c => c.DepartementID) // Foreign key property
               .OnDelete(DeleteBehavior.NoAction); // No action on deletion

            // relationship between Committee and User


            modelBuilder.Entity<Committee_M>()
            .ToTable("Committees")
            .HasOne(c => c.Responsible)
            .WithMany()
            .HasForeignKey(c => c.ResponsibleID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Committee_M>()
              .ToTable("Committees")
              .HasOne(c => c.Replacement)
              .WithMany()
              .HasForeignKey(c => c.ReplacementID)
              .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Committee_M>()
            .ToTable("Committees")
            .HasOne(c => c.ResponsibleTitle)
            .WithMany()
            .HasForeignKey(c => c.ResponsibleTitleID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeamLeader_M>()
            .ToTable("TeamLeaders")
              .HasOne(tl => tl.N_Area) // Committee has one Area
              .WithMany() // Area has many Committees
              .HasForeignKey(tl => tl.AreaID) // Foreign key property
              .OnDelete(DeleteBehavior.NoAction); // No action on deletion


            modelBuilder.Entity<TeamLeader_M>()
             .ToTable("TeamLeaders")
              .HasOne(tl => tl.N_Project) // Committee has one Area
              .WithMany() // Area has many Committees
              .HasForeignKey(tl => tl.ProjectID) // Foreign key property
              .OnDelete(DeleteBehavior.NoAction); // No action on deletion


            //modelBuilder.Entity<TeamLeader_M>()
            // .ToTable("TeamLeaders")
            //  .HasOne(tl => tl.N_User)
            //  .WithMany() 
            //  .HasForeignKey(tl => tl.UserID) // Foreign key property
            //  .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<User>()
                .HasOne(u => u.Leader)
                .WithMany(tl => tl.N_User)
                .HasForeignKey(u => u.LeaderId).IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Departement_M>()
                .ToTable("departement")
                .HasOne(de => de.BuildingID)
                .WithMany()
                .HasForeignKey(de => de.PlantID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SupervorM>()
                .ToTable("SupervorM")
                .HasOne(su => su.TEID)
                .WithMany()
                .HasForeignKey(su => su.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasOne(u => u.BuildingID)
                .WithMany()
                .HasForeignKey(u => u.PlantID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasOne(u => u.Name_Departement)
                .WithMany()
                .HasForeignKey(u => u.DepartementID);

            modelBuilder.Entity<Area_M>()
               .ToTable("Area")
               .HasOne(a => a.BuildingID)
               .WithMany()
               .HasForeignKey(a => a.PlantID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Machine_M>()
               .ToTable("Machine")
               .HasOne(m => m.Project_Name)
               .WithMany()
               .HasForeignKey(m => m.ProjectID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Project_M>()
               .ToTable("Project")
               .HasOne(p => p.Name_Area)
               .WithMany()
               .HasForeignKey(p => p.AreaID)
               .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Idea_M>()
                .ToTable("ideas")
                .HasOne(i => i.TEID)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
        // public DbSet<Asp_Net_Good_idea.Models.TeamLeader.TeamLeader_M> TeamLeader_M { get; set; }

    }
}
