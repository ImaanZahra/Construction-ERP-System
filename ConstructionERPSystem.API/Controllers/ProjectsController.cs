using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        // GET: api/projects
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            return Ok(_service.GetAllProjects());
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public IActionResult GetProjectById(int id)
        {
            var project = _service.GetProjectById(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        // POST: api/projects
        [HttpPost]
        public IActionResult AddProject(Project project)
        {
            _service.AddProject(project);

            return Ok("Project Added Successfully");
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, Project project)
        {
            if (id != project.ProjectId)
                return BadRequest();

            _service.UpdateProject(project);

            return Ok("Project Updated Successfully");
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            _service.DeleteProject(id);

            return Ok("Project Deleted Successfully");
        }
    }
}