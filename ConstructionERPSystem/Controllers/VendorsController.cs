
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
    "Procurement Officer"
)]
    public class VendorsController : Controller
    {
        private readonly DbHelper _db;

        public VendorsController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Vendor> vendors = new List<Vendor>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Vendors";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vendors.Add(new Vendor
                {
                    VendorId = Convert.ToInt32(reader["VendorId"]),
                    VendorName = reader["VendorName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Address = reader["Address"].ToString()
                });
            }

            return View(vendors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Vendor vendor)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Vendors
                            (VendorName, Phone, Email, Address)
                            VALUES
                            (@VendorName, @Phone, @Email, @Address)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@VendorName", vendor.VendorName);
            cmd.Parameters.AddWithValue("@Phone", vendor.Phone ?? "");
            cmd.Parameters.AddWithValue("@Email", vendor.Email ?? "");
            cmd.Parameters.AddWithValue("@Address", vendor.Address ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Vendor vendor = GetVendorById(id);
            if (vendor == null) return NotFound();

            return View(vendor);
        }

        public IActionResult Edit(int id)
        {
            Vendor vendor = GetVendorById(id);
            if (vendor == null) return NotFound();

            return View(vendor);
        }

        [HttpPost]
        public IActionResult Edit(Vendor vendor)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Vendors
                             SET VendorName = @VendorName,
                                 Phone = @Phone,
                                 Email = @Email,
                                 Address = @Address
                             WHERE VendorId = @VendorId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@VendorId", vendor.VendorId);
            cmd.Parameters.AddWithValue("@VendorName", vendor.VendorName);
            cmd.Parameters.AddWithValue("@Phone", vendor.Phone ?? "");
            cmd.Parameters.AddWithValue("@Email", vendor.Email ?? "");
            cmd.Parameters.AddWithValue("@Address", vendor.Address ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Vendors WHERE VendorId = @VendorId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@VendorId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Vendor GetVendorById(int id)
        {
            Vendor vendor = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Vendors WHERE VendorId = @VendorId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@VendorId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                vendor = new Vendor
                {
                    VendorId = Convert.ToInt32(reader["VendorId"]),
                    VendorName = reader["VendorName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Address = reader["Address"].ToString()
                };
            }

            return vendor;
        }
    }
}