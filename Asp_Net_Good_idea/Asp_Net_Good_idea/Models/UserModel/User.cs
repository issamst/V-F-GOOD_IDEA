using Asp_Net_Good_idea.Models.Committee;
using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.Plant;
using Asp_Net_Good_idea.Models.TeamLeader;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp_Net_Good_idea.Models.UserModel
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string TEID { get; set; }
        public string Badge_id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int RoleID { get; set; }
        public User_Role Name_Role { get; set; }
        public int PlantID { get; set; }
        public Plant_M BuildingID { get; set; }
        public int DepartementID { get; set; }
        public Departement_M Name_Departement { get; set; }
        public int TitleID { get; set; }
        public User_Title Name_Title { get; set; }
        public string Status { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpiryTime { get; set; }
        public DateTime RegisterTime { get; set; }
        public string Request { get; set; }
        public string CommenterDelete { get; set; }
        public string Supervisor { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }

        public int? LeaderId { get; set; }

        [ForeignKey("LeaderId")]

        public TeamLeader_M Leader { get; set; } //= null;

        public string RequestPassword { get; set; }


    }
}


