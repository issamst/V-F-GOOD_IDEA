using Asp_Net_Good_idea.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace Asp_Net_Good_idea.Models.idea
{
    public class Idea_M
    {

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionSituation { get; set; }
        public string DescriptionSolution { get; set; }
        public List<string> Name_Area { get; set; }
        public List<string> Project_Name { get; set; }
        public List<string> Name_Machine { get; set; }
        public string Name_Impact { get; set; }
        public string Status { get; set; }
        //file
        public List<string> FileIdeaPath { get; set; }
        public List<string> FileSituationPath { get; set; }
        public List<string> FileSolutionPath { get; set; }
        public List<int> Team_Leader_Approved { get; set; }


        public string CommenterTLApproved { get; set; }
        public List<int> Team_Leader_Rejected { get; set; }
        public string CommenterTLRejected { get; set; }
        public List<int> Committee_Approved { get; set; }
        public string CommenterCOMApproved { get; set; }
        public List<int> Committee_Rejected { get; set; }
        public string CommenterCOMRejected { get; set; }
        public string CommenterDisabled { get; set; }
        public DateTime Date_Create { get; set; }
        public DateTime Date_Update { get; set; }
        public DateTime Date_delete { get; set; }
        public int UserId { get; set; } // Change the type to int
        public User TEID { get; set; }

    }
}
