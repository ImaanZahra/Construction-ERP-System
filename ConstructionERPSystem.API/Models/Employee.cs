namespace ConstructionERPSystem.API.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
    }
}