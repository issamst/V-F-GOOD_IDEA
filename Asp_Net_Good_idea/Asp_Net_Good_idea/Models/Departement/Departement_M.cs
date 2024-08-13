using Asp_Net_Good_idea.Models.Plant;
using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.Departement
{
    public class Departement_M
    {
        [Key]
        public int Id { get; set; }
        public string Name_Departement { get; set; }

        public string TEID { get; set; }
        public string FullName { get; set; }
        public string Manger_Email { get; set; }
        public string CommenterDelete { get; set; }
        public int PlantID { get; set; }
        public Plant_M BuildingID { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }

    }
}