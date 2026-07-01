using ConstructionERPSystem.API.Models;

namespace ConstructionERPSystem.API.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks();
        TaskItem GetTaskById(int id);
        void AddTask(TaskItem task);
        void UpdateTask(TaskItem task);
        void DeleteTask(int id);
    }
}