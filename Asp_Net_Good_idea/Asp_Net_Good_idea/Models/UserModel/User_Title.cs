using Asp_Net_Good_idea.Models.Departement;
using Asp_Net_Good_idea.Models.Plant;
namespace Asp_Net_Good_idea.Models.UserModel
{
    public class User_Title
    {
        public int Id { get; set; }
        public string Name_Title { get; set; }
        public string CommenterDelete { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }

    }
}
