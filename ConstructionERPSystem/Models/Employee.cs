namespace ConstructionERPSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public int? UserId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Designation { get; set; }

        public decimal? Salary { get; set; }

        public string EmployeeType { get; set; }

        public DateTime? JoiningDate { get; set; }

        public string UserName { get; set; }
    }
}