using Asp_Net_Good_idea.Models.Area;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.Plant;
using Asp_Net_Good_idea.Models.UserModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;


namespace Asp_Net_Good_idea.Models.Committee
{
    public class Committee_M
    {

        [Key]     
        public int Id { get; set; }
        public string Name_Committee { get; set; }
        //public string type { get; set; }

        public int PlantID { get; set; }
        public Plant_M N_Plant { get; set; }

        public int AreaID { get; set; } // Foreign key to Area_M
        public Area_M N_Area { get; set; } // Navigation property

        public int DepartementID { get; set; }
        public Departement_M N_Departement { get; set; }
   
        public int ReplacementID { get; set; }
        [ForeignKey("ReplacementID")]
        public User Replacement { get; set; }   // Navigation property 

        public int ResponsibleID { get; set; }
        [ForeignKey("ResponsibleID")]
        public User Responsible { get; set; }   // Navigation property

        [ForeignKey("ResponsibleTitleID")]

        public int ResponsibleTitleID { get; set; }
        public User_Title ResponsibleTitle { get; set; }


        public string CommenterDelete { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }

    }
}
