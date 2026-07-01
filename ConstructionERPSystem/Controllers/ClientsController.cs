using ConstructionERPSystem.Filters;
using ConstructionERPSystem.Data;
using ConstructionERPSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.Controllers
{
    [RoleAuthorize("Admin", "Project Manager")]
    public class ClientsController : Controller
    {
        private readonly DbHelper _db;

        public ClientsController(DbHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Client> clients = new List<Client>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Clients";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                clients.Add(new Client
                {
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    ClientName = reader["ClientName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Address = reader["Address"].ToString()
                });
            }

            return View(clients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"INSERT INTO Clients 
                            (ClientName, Phone, Email, Address)
                            VALUES 
                            (@ClientName, @Phone, @Email, @Address)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ClientName", client.ClientName);
            cmd.Parameters.AddWithValue("@Phone", client.Phone ?? "");
            cmd.Parameters.AddWithValue("@Email", client.Email ?? "");
            cmd.Parameters.AddWithValue("@Address", client.Address ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            Client client = GetClientById(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        public IActionResult Edit(int id)
        {
            Client client = GetClientById(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        public IActionResult Edit(Client client)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = @"UPDATE Clients 
                             SET ClientName = @ClientName,
                                 Phone = @Phone,
                                 Email = @Email,
                                 Address = @Address
                             WHERE ClientId = @ClientId";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ClientId", client.ClientId);
            cmd.Parameters.AddWithValue("@ClientName", client.ClientName);
            cmd.Parameters.AddWithValue("@Phone", client.Phone ?? "");
            cmd.Parameters.AddWithValue("@Email", client.Email ?? "");
            cmd.Parameters.AddWithValue("@Address", client.Address ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Clients WHERE ClientId = @ClientId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ClientId", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        private Client GetClientById(int id)
        {
            Client client = null;

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Clients WHERE ClientId = @ClientId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ClientId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                client = new Client
                {
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    ClientName = reader["ClientName"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Address = reader["Address"].ToString()
                };
            }

            return client;
        }
    }
}