using ConstructionERPSystem.API.Data;
using ConstructionERPSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.API.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DbHelper _db;

        public ProjectRepository(DbHelper db)
        {
            _db = db;
        }

        public List<Project> GetAllProjects()
        {
            var projects = new List<Project>();

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
                projects.Add(MapProject(reader));
            }

            return projects;
        }

        public Project GetProjectById(int id)
        {
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
                return MapProject(reader);

            return null;
        }

        public void AddProject(Project project)
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
        }

        public void UpdateProject(Project project)
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
        }

        public void DeleteProject(int id)
        {
            using var con = _db.GetConnection();
            con.Open();

            string query = "DELETE FROM Projects WHERE ProjectId = @ProjectId";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ProjectId", id);

            cmd.ExecuteNonQuery();
        }

        private Project MapProject(SqlDataReader reader)
        {
            return new Project
            {
                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                ClientId = Convert.ToInt32(reader["ClientId"]),
                ClientName = reader["ClientName"].ToString(),
                ProjectName = reader["ProjectName"].ToString(),
                Description = reader["Description"] == DBNull.Value ? "" : reader["Description"].ToString(),
                StartDate = reader["StartDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["StartDate"]),
                EndDate = reader["EndDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["EndDate"]),
                Budget = reader["Budget"] == DBNull.Value ? null : Convert.ToDecimal(reader["Budget"]),
                Status = reader["Status"] == DBNull.Value ? "" : reader["Status"].ToString()
            };
        }
    }
}