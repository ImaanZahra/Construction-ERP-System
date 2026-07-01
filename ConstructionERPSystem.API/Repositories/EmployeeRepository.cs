using ConstructionERPSystem.API.Data;
using ConstructionERPSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DbHelper _db;

        public EmployeeRepository(DbHelper db)
        {
            _db = db;
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using var con = _db.GetConnection();
            con.Open();

            using SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    FullName = reader["FullName"].ToString(),
                    Designation = reader["Designation"].ToString(),
                    EmployeeType = reader["EmployeeType"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    Status = reader["Status"].ToString()
                });
            }

            return employees;
        }

        public Employee GetEmployeeById(int id)
        {
            return GetAllEmployees().FirstOrDefault(x => x.EmployeeId == id);
        }

        public void AddEmployee(Employee employee) { }

        public void UpdateEmployee(Employee employee) { }

        public void DeleteEmployee(int id) { }
    }
}