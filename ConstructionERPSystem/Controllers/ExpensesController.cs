
using ConstructionERPSystem.Data;
using ConstructionERPSystem.Filters;
using ConstructionERPSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace ConstructionERP.Controllers
{
    [RoleAuthorize(
    "Admin",
    "Accountant",
    "Project Manager"
)]
    public class ExpensesController : Controller
    {
        private readonly DbHelper _db;

        public ExpensesController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Expense> expenses = new List<Expense>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT e.*, p.ProjectName
                FROM Expenses e
                INNER JOIN Projects p ON e.ProjectId = p.ProjectId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                expenses.Add(new Expense
                {
                    ExpenseId = Convert.ToInt32(reader["ExpenseId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    ExpenseType = reader["ExpenseType"].ToString(),
                    Amount = Convert.ToDecimal(reader["Amount"]),
                    ExpenseDate = reader["ExpenseDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpenseDate"]),
                    Description = reader["Description"].ToString()
                });
            }

            return View(expenses);
        }

        public IActionResult Create()
        {
            LoadProjects();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Expenses
                            (ProjectId, ExpenseType, Amount, ExpenseDate, Description)
                            VALUES
                            (@ProjectId, @ExpenseType, @Amount, @ExpenseDate, @Description)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProjectId", expense.ProjectId);
            cmd.Parameters.AddWithValue("@ExpenseType", expense.ExpenseType ?? "");
            cmd.Parameters.AddWithValue("@Amount", expense.Amount);
            cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", expense.Description ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Expense expense = GetExpenseById(id);
            if (expense == null) return NotFound();

            return View(expense);
        }

        public IActionResult Edit(int id)
        {
            Expense expense = GetExpenseById(id);
            if (expense == null) return NotFound();

            LoadProjects();
            return View(expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense expense)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Expenses
                             SET ProjectId = @ProjectId,
                                 ExpenseType = @ExpenseType,
                                 Amount = @Amount,
                                 ExpenseDate = @ExpenseDate,
                                 Description = @Description
                             WHERE ExpenseId = @ExpenseId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ExpenseId", expense.ExpenseId);
            cmd.Parameters.AddWithValue("@ProjectId", expense.ProjectId);
            cmd.Parameters.AddWithValue("@ExpenseType", expense.ExpenseType ?? "");
            cmd.Parameters.AddWithValue("@Amount", expense.Amount);
            cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", expense.Description ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Expenses WHERE ExpenseId = @ExpenseId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ExpenseId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Expense GetExpenseById(int id)
        {
            Expense expense = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT e.*, p.ProjectName
                FROM Expenses e
                INNER JOIN Projects p ON e.ProjectId = p.ProjectId
                WHERE e.ExpenseId = @ExpenseId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ExpenseId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                expense = new Expense
                {
                    ExpenseId = Convert.ToInt32(reader["ExpenseId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    ExpenseType = reader["ExpenseType"].ToString(),
                    Amount = Convert.ToDecimal(reader["Amount"]),
                    ExpenseDate = reader["ExpenseDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ExpenseDate"]),
                    Description = reader["Description"].ToString()
                };
            }

            return expense;
        }

        private void LoadProjects()
        {
            List<SelectListItem> projects = new List<SelectListItem>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT ProjectId, ProjectName FROM Projects";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                projects.Add(new SelectListItem
                {
                    Value = reader["ProjectId"].ToString(),
                    Text = reader["ProjectName"].ToString()
                });
            }

            ViewBag.Projects = projects;
        }
    }
}