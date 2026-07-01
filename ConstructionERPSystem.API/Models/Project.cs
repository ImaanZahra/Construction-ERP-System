namespace ConstructionERPSystem.API.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public int ClientId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
    }
}