using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Repositories;

namespace ConstructionERPSystem.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public List<TaskItem> GetAllTasks() => _repository.GetAllTasks();

        public TaskItem GetTaskById(int id) => _repository.GetTaskById(id);

        public void AddTask(TaskItem task) => _repository.AddTask(task);

        public void UpdateTask(TaskItem task) => _repository.UpdateTask(task);

        public void DeleteTask(int id) => _repository.DeleteTask(id);
    }
}