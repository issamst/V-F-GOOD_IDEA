using Asp_Net_Good_idea.Models.Committee;
using Asp_Net_Good_idea.Models.Plant;
using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.Area
{
    public class Area_M
    {
        [Key]
        public int Id { get; set; }
        public string Name_Area { get; set; }
        public int PlantID { get; set; }
        public Plant_M BuildingID { get; set; }
        public string CommenterDelete { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }


    }
}
