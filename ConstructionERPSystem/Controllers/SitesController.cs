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
    "Site Engineer"
)]
    public class SitesController : Controller
    {
        private readonly DbHelper _db;

        public SitesController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Site> sites = new List<Site>();

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT s.*, p.ProjectName
                FROM Sites s
                INNER JOIN Projects p ON s.ProjectId = p.ProjectId";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                sites.Add(new Site
                {
                    SiteId = Convert.ToInt32(reader["SiteId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    SiteName = reader["SiteName"].ToString(),
                    Location = reader["Location"].ToString()
                });
            }

            return View(sites);
        }

        public IActionResult Create()
        {
            LoadProjects();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Site site)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Sites
                            (ProjectId, SiteName, Location)
                            VALUES
                            (@ProjectId, @SiteName, @Location)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProjectId", site.ProjectId);
            cmd.Parameters.AddWithValue("@SiteName", site.SiteName);
            cmd.Parameters.AddWithValue("@Location", site.Location ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Site site = GetSiteById(id);

            if (site == null)
                return NotFound();

            return View(site);
        }

        public IActionResult Edit(int id)
        {
            Site site = GetSiteById(id);

            if (site == null)
                return NotFound();

            LoadProjects();
            return View(site);
        }

        [HttpPost]
        public IActionResult Edit(Site site)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Sites
                             SET ProjectId = @ProjectId,
                                 SiteName = @SiteName,
                                 Location = @Location
                             WHERE SiteId = @SiteId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@SiteId", site.SiteId);
            cmd.Parameters.AddWithValue("@ProjectId", site.ProjectId);
            cmd.Parameters.AddWithValue("@SiteName", site.SiteName);
            cmd.Parameters.AddWithValue("@Location", site.Location ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Sites WHERE SiteId = @SiteId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@SiteId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Site GetSiteById(int id)
        {
            Site site = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = @"
                SELECT s.*, p.ProjectName
                FROM Sites s
                INNER JOIN Projects p ON s.ProjectId = p.ProjectId
                WHERE s.SiteId = @SiteId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@SiteId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                site = new Site
                {
                    SiteId = Convert.ToInt32(reader["SiteId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    ProjectName = reader["ProjectName"].ToString(),
                    SiteName = reader["SiteName"].ToString(),
                    Location = reader["Location"].ToString()
                };
            }

            return site;
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