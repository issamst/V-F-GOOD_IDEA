using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.Area
{
    public class Machine_M
    {
        [Key]
        public int Id { get; set; }
        public string Machine_Name { get; set; }
        public string Building_ID { get; set; }
        public string AreaID { get; set; }
        public int ProjectID { get; set; }
        public Project_M Project_Name { get; set; }
        public string CommenterDelete { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }
    }
}
