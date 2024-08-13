namespace Asp_Net_Good_idea.Models.Dto
{
    public class IdeaDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionSituation { get; set; }
        public string DescriptionSolution { get; set; }
        public string Name_Area { get; set; }
        public string Project_Name { get; set; }
        public string Name_Machine { get; set; }
        public string Name_Impact { get; set; }
        public IFormFile FileSituation { get; set; }
        public IFormFile FileSolution { get; set; }
        public IFormFile FileIdea { get; set; }
        public int UserId { get; set; }
    }

}