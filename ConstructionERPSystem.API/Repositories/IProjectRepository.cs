using ConstructionERPSystem.API.Models;

namespace ConstructionERPSystem.API.Repositories
{
    public interface IProjectRepository
    {
        List<Project> GetAllProjects();
        Project GetProjectById(int id);
        void AddProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(int id);
    }
}