using Asp_Net_Good_idea.Models.Departement;
using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.Plant
{
    public class Plant_M
    {
        [Key]
        public int Id { get; set; }
        public string BuildingID { get; set; }
        public string SapBuildingNumber { get; set; }
        public string BU { get; set; }
        public string Location { get; set; }
        public string CommenterDelete { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_Delete { get; set;}

    }
}
