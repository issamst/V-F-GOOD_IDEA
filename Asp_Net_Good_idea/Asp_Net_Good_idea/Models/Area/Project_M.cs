using Asp_Net_Good_idea.Models.Plant;
using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.Area
{
    public class Project_M
    {
        [Key]
        public int Id { get; set; }
        public string Project_Name { get; set; }
        public string Building_ID { get; set; }
        public int AreaID { get; set; }
        public Area_M Name_Area { get; set; }
        public string CommenterDelete { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }
    }
}
