namespace ConstructionERPSystem.Models
{
    public class Site
    {
        public int SiteId { get; set; }

        public int ProjectId { get; set; }

        public string SiteName { get; set; }

        public string Location { get; set; }

        public string ProjectName { get; set; }
    }
}