using ConstructionERPSystem.Filters;
using ConstructionERPSystem.Models;
using ConstructionERPSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.Controllers
{
    [RoleAuthorize("Admin")]
    public class UsersController : Controller
    {
        private readonly DbHelper _db;

        public UsersController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<User> users = new List<User>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Users";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    FullName = reader["FullName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PasswordHash = reader["PasswordHash"].ToString(),
                    Role = reader["Role"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Status = reader["Status"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                });
            }

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Users
                            (FullName, Email, PasswordHash, Role, Phone, Status)
                            VALUES
                            (@FullName, @Email, @PasswordHash, @Role, @Phone, @Status)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Role", user.Role);
            cmd.Parameters.AddWithValue("@Phone", user.Phone ?? "");
            cmd.Parameters.AddWithValue("@Status", user.Status ?? "Active");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            User user = GetUserById(id);
            if (user == null) return NotFound();

            return View(user);
        }

        public IActionResult Edit(int id)
        {
            User user = GetUserById(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Users
                             SET FullName = @FullName,
                                 Email = @Email,
                                 PasswordHash = @PasswordHash,
                                 Role = @Role,
                                 Phone = @Phone,
                                 Status = @Status
                             WHERE UserId = @UserId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@UserId", user.UserId);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Role", user.Role);
            cmd.Parameters.AddWithValue("@Phone", user.Phone ?? "");
            cmd.Parameters.AddWithValue("@Status", user.Status ?? "Active");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Users WHERE UserId = @UserId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private User GetUserById(int id)
        {
            User user = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Users WHERE UserId = @UserId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                user = new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    FullName = reader["FullName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PasswordHash = reader["PasswordHash"].ToString(),
                    Role = reader["Role"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Status = reader["Status"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                };
            }

            return user;
        }
    }
}