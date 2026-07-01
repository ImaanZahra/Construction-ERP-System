
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
    "Site Engineer",
    "Contractor",
    "Worker"
)]
    public class TasksController : Controller
    {
        private readonly DbHelper _db;

        public TasksController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<TaskModel> tasks = new List<TaskModel>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT t.*, p.ProjectName, s.SiteName, e.FullName AS EmployeeName
                FROM Tasks t
                INNER JOIN Projects p ON t.ProjectId = p.ProjectId
                LEFT JOIN Sites s ON t.SiteId = s.SiteId
                LEFT JOIN Employees e ON t.EmployeeId = e.EmployeeId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new TaskModel
                {
                    TaskId = Convert.ToInt32(reader["TaskId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    SiteId = reader["SiteId"] == DBNull.Value ? null : Convert.ToInt32(reader["SiteId"]),
                    EmployeeId = reader["EmployeeId"] == DBNull.Value ? null : Convert.ToInt32(reader["EmployeeId"]),
                    TaskTitle = reader["TaskTitle"].ToString(),
                    Description = reader["Description"].ToString(),
                    StartDate = reader["StartDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["StartDate"]),
                    DueDate = reader["DueDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["DueDate"]),
                    Priority = reader["Priority"].ToString(),
                    Status = reader["Status"].ToString(),
                    Progress = Convert.ToInt32(reader["Progress"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    SiteName = reader["SiteName"] == DBNull.Value ? "No Site" : reader["SiteName"].ToString(),
                    EmployeeName = reader["EmployeeName"] == DBNull.Value ? "Not Assigned" : reader["EmployeeName"].ToString()
                });
            }

            return View(tasks);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskModel task)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Tasks
                            (ProjectId, SiteId, EmployeeId, TaskTitle, Description, StartDate, DueDate, Priority, Status, Progress)
                            VALUES
                            (@ProjectId, @SiteId, @EmployeeId, @TaskTitle, @Description, @StartDate, @DueDate, @Priority, @Status, @Progress)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            cmd.Parameters.AddWithValue("@SiteId", task.SiteId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EmployeeId", task.EmployeeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
            cmd.Parameters.AddWithValue("@Description", task.Description ?? "");
            cmd.Parameters.AddWithValue("@StartDate", task.StartDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DueDate", task.DueDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Priority", task.Priority ?? "Medium");
            cmd.Parameters.AddWithValue("@Status", task.Status ?? "Pending");
            cmd.Parameters.AddWithValue("@Progress", task.Progress);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            TaskModel task = GetTaskById(id);
            if (task == null) return NotFound();

            return View(task);
        }

        public IActionResult Edit(int id)
        {
            TaskModel task = GetTaskById(id);
            if (task == null) return NotFound();

            LoadDropdowns();
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskModel task)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Tasks
                             SET ProjectId = @ProjectId,
                                 SiteId = @SiteId,
                                 EmployeeId = @EmployeeId,
                                 TaskTitle = @TaskTitle,
                                 Description = @Description,
                                 StartDate = @StartDate,
                                 DueDate = @DueDate,
                                 Priority = @Priority,
                                 Status = @Status,
                                 Progress = @Progress
                             WHERE TaskId = @TaskId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@TaskId", task.TaskId);
            cmd.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            cmd.Parameters.AddWithValue("@SiteId", task.SiteId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EmployeeId", task.EmployeeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TaskTitle", task.TaskTitle);
            cmd.Parameters.AddWithValue("@Description", task.Description ?? "");
            cmd.Parameters.AddWithValue("@StartDate", task.StartDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DueDate", task.DueDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Priority", task.Priority ?? "Medium");
            cmd.Parameters.AddWithValue("@Status", task.Status ?? "Pending");
            cmd.Parameters.AddWithValue("@Progress", task.Progress);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@TaskId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private TaskModel GetTaskById(int id)
        {
            TaskModel task = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT t.*, p.ProjectName, s.SiteName, e.FullName AS EmployeeName
                FROM Tasks t
                INNER JOIN Projects p ON t.ProjectId = p.ProjectId
                LEFT JOIN Sites s ON t.SiteId = s.SiteId
                LEFT JOIN Employees e ON t.EmployeeId = e.EmployeeId
                WHERE t.TaskId = @TaskId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@TaskId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                task = new TaskModel
                {
                    TaskId = Convert.ToInt32(reader["TaskId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    SiteId = reader["SiteId"] == DBNull.Value ? null : Convert.ToInt32(reader["SiteId"]),
                    EmployeeId = reader["EmployeeId"] == DBNull.Value ? null : Convert.ToInt32(reader["EmployeeId"]),
                    TaskTitle = reader["TaskTitle"].ToString(),
                    Description = reader["Description"].ToString(),
                    StartDate = reader["StartDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["StartDate"]),
                    DueDate = reader["DueDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["DueDate"]),
                    Priority = reader["Priority"].ToString(),
                    Status = reader["Status"].ToString(),
                    Progress = Convert.ToInt32(reader["Progress"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    SiteName = reader["SiteName"] == DBNull.Value ? "No Site" : reader["SiteName"].ToString(),
                    EmployeeName = reader["EmployeeName"] == DBNull.Value ? "Not Assigned" : reader["EmployeeName"].ToString()
                };
            }

            return task;
        }

        private void LoadDropdowns()
        {
            ViewBag.Projects = GetDropdown("SELECT ProjectId, ProjectName FROM Projects", "ProjectId", "ProjectName");
            ViewBag.Sites = GetDropdown("SELECT SiteId, SiteName FROM Sites", "SiteId", "SiteName");
            ViewBag.Employees = GetDropdown("SELECT EmployeeId, FullName FROM Employees", "EmployeeId", "FullName");
        }

        private List<SelectListItem> GetDropdown(string query, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using var con = _db.GetConnection();
            con.Open();

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = reader[valueField].ToString(),
                    Text = reader[textField].ToString()
                });
            }

            return list;
        }
    }
}