using ConstructionERPSystem.API.Data;
using ConstructionERPSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbHelper _db;

        public UserRepository(DbHelper db)
        {
            _db = db;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Users";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(MapUser(reader));
            }

            return users;
        }

        public User GetUserById(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Users WHERE UserId = @UserId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapUser(reader);

            return null;
        }

        public void AddUser(User user)
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
        }

        public void UpdateUser(User user)
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
        }

        public void DeleteUser(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Users WHERE UserId = @UserId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", id);

            cmd.ExecuteNonQuery();
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                FullName = reader["FullName"].ToString(),
                Email = reader["Email"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                Role = reader["Role"].ToString(),
                Phone = reader["Phone"] == DBNull.Value ? "" : reader["Phone"].ToString(),
                Status = reader["Status"].ToString(),
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}