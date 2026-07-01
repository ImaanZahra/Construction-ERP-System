namespace ConstructionERPSystem.Models
{
    public class TaskModel
    {
        public int TaskId { get; set; }

        public int ProjectId { get; set; }

        public int? SiteId { get; set; }

        public int? EmployeeId { get; set; }

        public string TaskTitle { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public int Progress { get; set; }

        public string ProjectName { get; set; }

        public string SiteName { get; set; }

        public string EmployeeName { get; set; }
    }
}