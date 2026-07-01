using ConstructionERPSystem.Data;
using ConstructionERPSystem.Filters;
using ConstructionERPSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.Controllers
{
    [RoleAuthorize(
    "Admin",
    "Project Manager",
    "HR Manager",
    "Contractor"
)]
    public class EmployeesController : Controller
    {
        private readonly DbHelper _db;

        public EmployeesController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT e.*, u.FullName AS UserName
                FROM Employees e
                LEFT JOIN Users u ON e.UserId = u.UserId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    UserId = reader["UserId"] == DBNull.Value ? null : Convert.ToInt32(reader["UserId"]),
                    UserName = reader["UserName"] == DBNull.Value ? "No Login Account" : reader["UserName"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Designation = reader["Designation"].ToString(),
                    Salary = reader["Salary"] == DBNull.Value ? null : Convert.ToDecimal(reader["Salary"]),
                    EmployeeType = reader["EmployeeType"].ToString(),
                    JoiningDate = reader["JoiningDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["JoiningDate"])
                });
            }

            return View(employees);
        }

        public IActionResult Create()
        {
            LoadUsers();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Employees
                            (UserId, FullName, Phone, Designation, Salary, EmployeeType, JoiningDate)
                            VALUES
                            (@UserId, @FullName, @Phone, @Designation, @Salary, @EmployeeType, @JoiningDate)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@UserId", employee.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FullName", employee.FullName);
            cmd.Parameters.AddWithValue("@Phone", employee.Phone ?? "");
            cmd.Parameters.AddWithValue("@Designation", employee.Designation ?? "");
            cmd.Parameters.AddWithValue("@Salary", employee.Salary ?? 0);
            cmd.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType ?? "");
            cmd.Parameters.AddWithValue("@JoiningDate", employee.JoiningDate ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Employee employee = GetEmployeeById(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        public IActionResult Edit(int id)
        {
            Employee employee = GetEmployeeById(id);

            if (employee == null)
                return NotFound();

            LoadUsers();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Employees
                             SET UserId = @UserId,
                                 FullName = @FullName,
                                 Phone = @Phone,
                                 Designation = @Designation,
                                 Salary = @Salary,
                                 EmployeeType = @EmployeeType,
                                 JoiningDate = @JoiningDate
                             WHERE EmployeeId = @EmployeeId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
            cmd.Parameters.AddWithValue("@UserId", employee.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@FullName", employee.FullName);
            cmd.Parameters.AddWithValue("@Phone", employee.Phone ?? "");
            cmd.Parameters.AddWithValue("@Designation", employee.Designation ?? "");
            cmd.Parameters.AddWithValue("@Salary", employee.Salary ?? 0);
            cmd.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType ?? "");
            cmd.Parameters.AddWithValue("@JoiningDate", employee.JoiningDate ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@EmployeeId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Employee GetEmployeeById(int id)
        {
            Employee employee = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT e.*, u.FullName AS UserName
                FROM Employees e
                LEFT JOIN Users u ON e.UserId = u.UserId
                WHERE e.EmployeeId = @EmployeeId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@EmployeeId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                employee = new Employee
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    UserId = reader["UserId"] == DBNull.Value ? null : Convert.ToInt32(reader["UserId"]),
                    UserName = reader["UserName"] == DBNull.Value ? "No Login Account" : reader["UserName"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Designation = reader["Designation"].ToString(),
                    Salary = reader["Salary"] == DBNull.Value ? null : Convert.ToDecimal(reader["Salary"]),
                    EmployeeType = reader["EmployeeType"].ToString(),
                    JoiningDate = reader["JoiningDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["JoiningDate"])
                };
            }

            return employee;
        }

        private void LoadUsers()
        {
            List<SelectListItem> users = new List<SelectListItem>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT UserId, FullName FROM Users";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new SelectListItem
                {
                    Value = reader["UserId"].ToString(),
                    Text = reader["FullName"].ToString()
                });
            }

            ViewBag.Users = users;
        }
    }
}