using Asp_Net_Good_idea.Models.Plant;
using Asp_Net_Good_idea.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.supervisorManager
{
    public class SupervorM
    {
        [Key]
        public int Id { get; set; }
        public string Speciality { get; set; }

        public int UserID { get; set; }
        public User TEID { get; set; }
        public string CommenterDisabled { get; set; }
        
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }
    }
}
