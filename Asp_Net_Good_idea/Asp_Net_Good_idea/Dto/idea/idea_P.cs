namespace Asp_Net_Good_idea.Dto.idea
{
    public class idea_P
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile FileIdea { get; set; }
        public IFormFile FileSituation { get; set; }
        public IFormFile FileSolution { get; set; }
        public string DescriptionSituation { get; set; }
        public string DescriptionSolution { get; set; }
        public string Status { get; set; }
        public string NameArea { get; set; }
        public string ProjectName { get; set; }
        public string NameImpact { get; set; }
        public DateTime DueDate { get; set; }
        public string CommenterDelete { get; set; }
        public int UserId { get; set; }
    }
}
