
using ConstructionERPSystem.Data;
using ConstructionERPSystem.Filters;
using ConstructionERPSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.Controllers
{
    [RoleAuthorize(
    "Admin",
    "Store Manager",
    "Procurement Officer",
    "Project Manager"
)]
    public class MaterialsController : Controller
    {
        private readonly DbHelper _db;

        public MaterialsController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Material> materials = new List<Material>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Materials";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                materials.Add(new Material
                {
                    MaterialId = Convert.ToInt32(reader["MaterialId"]),
                    MaterialName = reader["MaterialName"].ToString(),
                    Unit = reader["Unit"].ToString(),
                    CurrentStock = Convert.ToInt32(reader["CurrentStock"]),
                    MinimumStock = Convert.ToInt32(reader["MinimumStock"])
                });
            }

            return View(materials);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Material material)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Materials
                            (MaterialName, Unit, CurrentStock, MinimumStock)
                            VALUES
                            (@MaterialName, @Unit, @CurrentStock, @MinimumStock)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@MaterialName", material.MaterialName);
            cmd.Parameters.AddWithValue("@Unit", material.Unit ?? "");
            cmd.Parameters.AddWithValue("@CurrentStock", material.CurrentStock);
            cmd.Parameters.AddWithValue("@MinimumStock", material.MinimumStock);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Material material = GetMaterialById(id);
            if (material == null) return NotFound();

            return View(material);
        }

        public IActionResult Edit(int id)
        {
            Material material = GetMaterialById(id);
            if (material == null) return NotFound();

            return View(material);
        }

        [HttpPost]
        public IActionResult Edit(Material material)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Materials
                             SET MaterialName = @MaterialName,
                                 Unit = @Unit,
                                 CurrentStock = @CurrentStock,
                                 MinimumStock = @MinimumStock
                             WHERE MaterialId = @MaterialId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@MaterialId", material.MaterialId);
            cmd.Parameters.AddWithValue("@MaterialName", material.MaterialName);
            cmd.Parameters.AddWithValue("@Unit", material.Unit ?? "");
            cmd.Parameters.AddWithValue("@CurrentStock", material.CurrentStock);
            cmd.Parameters.AddWithValue("@MinimumStock", material.MinimumStock);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Materials WHERE MaterialId = @MaterialId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@MaterialId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Material GetMaterialById(int id)
        {
            Material material = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Materials WHERE MaterialId = @MaterialId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@MaterialId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                material = new Material
                {
                    MaterialId = Convert.ToInt32(reader["MaterialId"]),
                    MaterialName = reader["MaterialName"].ToString(),
                    Unit = reader["Unit"].ToString(),
                    CurrentStock = Convert.ToInt32(reader["CurrentStock"]),
                    MinimumStock = Convert.ToInt32(reader["MinimumStock"])
                };
            }

            return material;
        }
    }
}