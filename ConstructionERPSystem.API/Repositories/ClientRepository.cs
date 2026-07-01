using ConstructionERPSystem.API.Data;
using ConstructionERPSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.API.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DbHelper _db;

        public ClientRepository(DbHelper db)
        {
            _db = db;
        }

        public List<Client> GetAllClients()
        {
            var clients = new List<Client>();

            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Clients";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                clients.Add(MapClient(reader));
            }

            return clients;
        }

        public Client GetClientById(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "SELECT * FROM Clients WHERE ClientId = @ClientId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ClientId", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapClient(reader);

            return null;
        }

        public void AddClient(Client client)
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
        }

        public void UpdateClient(Client client)
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
        }

        public void DeleteClient(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Clients WHERE ClientId = @ClientId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ClientId", id);

            cmd.ExecuteNonQuery();
        }

        private Client MapClient(SqlDataReader reader)
        {
            return new Client
            {
                ClientId = Convert.ToInt32(reader["ClientId"]),
                ClientName = reader["ClientName"].ToString(),
                Phone = reader["Phone"] == DBNull.Value ? "" : reader["Phone"].ToString(),
                Email = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString(),
                Address = reader["Address"] == DBNull.Value ? "" : reader["Address"].ToString()
            };
        }
    }
}