using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Repositories;

namespace ConstructionERPSystem.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public List<Project> GetAllProjects()
        {
            return _repository.GetAllProjects();
        }

        public Project GetProjectById(int id)
        {
            return _repository.GetProjectById(id);
        }

        public void AddProject(Project project)
        {
            _repository.AddProject(project);
        }

        public void UpdateProject(Project project)
        {
            _repository.UpdateProject(project);
        }

        public void DeleteProject(int id)
        {
            _repository.DeleteProject(id);
        }
    }
}