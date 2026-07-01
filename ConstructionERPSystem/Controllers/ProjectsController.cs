
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
    "Client",
    "Site Engineer"
)]
    public class ProjectsController : Controller
    {
        private readonly DbHelper _db;

        public ProjectsController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Project> projects = new List<Project>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT p.*, c.ClientName
                FROM Projects p
                INNER JOIN Clients c ON p.ClientId = c.ClientId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                projects.Add(new Project
                {
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    ClientName = reader["ClientName"].ToString(),
                    ProjectName = reader["ProjectName"].ToString(),
                    Description = reader["Description"].ToString(),
                    StartDate = reader["StartDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["StartDate"]),
                    EndDate = reader["EndDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["EndDate"]),
                    Budget = reader["Budget"] == DBNull.Value ? null : Convert.ToDecimal(reader["Budget"]),
                    Status = reader["Status"].ToString()
                });
            }

            return View(projects);
        }

        public IActionResult Create()
        {
            LoadClients();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Projects
                            (ClientId, ProjectName, Description, StartDate, EndDate, Budget, Status)
                            VALUES
                            (@ClientId, @ProjectName, @Description, @StartDate, @EndDate, @Budget, @Status)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ClientId", project.ClientId);
            cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
            cmd.Parameters.AddWithValue("@Description", project.Description ?? "");
            cmd.Parameters.AddWithValue("@StartDate", project.StartDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", project.EndDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Budget", project.Budget ?? 0);
            cmd.Parameters.AddWithValue("@Status", project.Status ?? "Active");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Project project = GetProjectById(id);

            if (project == null)
                return NotFound();

            return View(project);
        }

        public IActionResult Edit(int id)
        {
            Project project = GetProjectById(id);

            if (project == null)
                return NotFound();

            LoadClients();
            return View(project);
        }

        [HttpPost]
        public IActionResult Edit(Project project)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Projects
                             SET ClientId = @ClientId,
                                 ProjectName = @ProjectName,
                                 Description = @Description,
                                 StartDate = @StartDate,
                                 EndDate = @EndDate,
                                 Budget = @Budget,
                                 Status = @Status
                             WHERE ProjectId = @ProjectId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
            cmd.Parameters.AddWithValue("@ClientId", project.ClientId);
            cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
            cmd.Parameters.AddWithValue("@Description", project.Description ?? "");
            cmd.Parameters.AddWithValue("@StartDate", project.StartDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", project.EndDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Budget", project.Budget ?? 0);
            cmd.Parameters.AddWithValue("@Status", project.Status ?? "Active");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Projects WHERE ProjectId = @ProjectId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ProjectId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Project GetProjectById(int id)
        {
            Project project = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT p.*, c.ClientName
                FROM Projects p
                INNER JOIN Clients c ON p.ClientId = c.ClientId
                WHERE p.ProjectId = @ProjectId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ProjectId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                project = new Project
                {
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    ClientName = reader["ClientName"].ToString(),
                    ProjectName = reader["ProjectName"].ToString(),
                    Description = reader["Description"].ToString(),
                    StartDate = reader["StartDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["StartDate"]),
                    EndDate = reader["EndDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["EndDate"]),
                    Budget = reader["Budget"] == DBNull.Value ? null : Convert.ToDecimal(reader["Budget"]),
                    Status = reader["Status"].ToString()
                };
            }

            return project;
        }

        private void LoadClients()
        {
            List<SelectListItem> clients = new List<SelectListItem>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT ClientId, ClientName FROM Clients";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                clients.Add(new SelectListItem
                {
                    Value = reader["ClientId"].ToString(),
                    Text = reader["ClientName"].ToString()
                });
            }

            ViewBag.Clients = clients;
        }
    }
}