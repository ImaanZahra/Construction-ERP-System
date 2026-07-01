namespace ConstructionERPSystem.API.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}