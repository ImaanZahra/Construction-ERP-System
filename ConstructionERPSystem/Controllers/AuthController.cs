using ConstructionERPSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly DbHelper _db;

        public AuthController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"SELECT UserId, FullName, Role 
                             FROM Users 
                             WHERE Email = @Email 
                             AND PasswordHash = @PasswordHash 
                             AND Status = 'Active'";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", password);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                HttpContext.Session.SetInt32("UserId", Convert.ToInt32(reader["UserId"]));
                HttpContext.Session.SetString("FullName", reader["FullName"].ToString());
                HttpContext.Session.SetString("Role", reader["Role"].ToString());

                string role = reader["Role"].ToString();

                if (role == "Admin")
                    return RedirectToAction("Admin", "Dashboard");

                if (role == "Project Manager")
                    return RedirectToAction("ProjectManager", "Dashboard");

                if (role == "Client")
                    return RedirectToAction("Client", "Dashboard");

                if (role == "Site Engineer")
                    return RedirectToAction("SiteEngineer", "Dashboard");

                if (role == "Store Manager")
                    return RedirectToAction("StoreManager", "Dashboard");
                if (role == "Procurement Officer")
                    return RedirectToAction("ProcurementOfficer", "Dashboard");

                if (role == "HR Manager")
                    return RedirectToAction("HRManager", "Dashboard");
                if (role == "Accountant")
                    return RedirectToAction("Accountant", "Dashboard");
                if (role == "Contractor")
                    return RedirectToAction("Contractor", "Dashboard");
                if (role == "Worker")
                    return RedirectToAction("Worker", "Dashboard");


                return RedirectToAction("Employee", "Dashboard");

               
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}