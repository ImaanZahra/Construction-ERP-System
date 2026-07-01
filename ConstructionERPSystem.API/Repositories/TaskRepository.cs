using ConstructionERPSystem.API.Data;
using ConstructionERPSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace ConstructionERPSystem.API.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DbHelper _db;

        public TaskRepository(DbHelper db)
        {
            _db = db;
        }

        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();

            using var con = _db.GetConnection();
            con.Open();

            using SqlCommand cmd = new SqlCommand("SELECT * FROM Tasks", con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new TaskItem
                {
                    TaskId = Convert.ToInt32(reader["TaskId"]),
                    TaskTitle = reader["TaskTitle"].ToString(),
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    ProjectId = Convert.ToInt32(reader["ProjectId"]),
                    Priority = reader["Priority"].ToString(),
                    Status = reader["Status"].ToString()
                });
            }

            return tasks;
        }

        public TaskItem GetTaskById(int id)
        {
            return GetAllTasks().FirstOrDefault(x => x.TaskId == id);
        }

        public void AddTask(TaskItem task) { }

        public void UpdateTask(TaskItem task) { }

        public void DeleteTask(int id) { }
    }
}