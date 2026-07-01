namespace ConstructionERPSystem.Models
{
    public class AttendanceModel
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }

        public int SiteId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public TimeSpan? CheckIn { get; set; }

        public TimeSpan? CheckOut { get; set; }

        public string Status { get; set; }

        public string EmployeeName { get; set; }

        public string SiteName { get; set; }
    }
}