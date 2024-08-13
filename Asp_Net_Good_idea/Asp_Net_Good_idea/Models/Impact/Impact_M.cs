using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.Impact
{
    public class Impact_M
    {

        [Key]
        public int Id { get; set; }
        public string Name_Impact { get; set; }
        public string Description { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }
        public string CommenterDelete { get; set; }
    }
}
