using ConstructionERPSystem.API.Models;

namespace ConstructionERPSystem.API.Services
{
    public interface IProjectService
    {
        List<Project> GetAllProjects();
        Project GetProjectById(int id);
        void AddProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(int id);
    }
}