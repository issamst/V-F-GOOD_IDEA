using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Asp_Net_Good_idea.Models.UserModel;
using Asp_Net_Good_idea.Models.Area;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.Plant;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;


namespace Asp_Net_Good_idea.Models.TeamLeader
{
    public class TeamLeader_M
    {

        [Key]
        public int Id { get; set; }
        public string Teamleader_Name { get; set; }
        public string shift { get; set; }

        public int AreaID { get; set; } // Foreign key to Area_M
        public Area_M N_Area { get; set; } // Navigation property

        public int ProjectID { get; set; }
        public Project_M N_Project { get; set; }

        [NotMapped]
        public int[]? UserID { get; set; }

        //public int UserID { get; set; }
        public ICollection<User> N_User { get; set; }   // Navigation property 


        public string CommenterDelete { get; set; }

        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }

    }
}