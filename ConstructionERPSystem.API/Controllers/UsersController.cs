using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_service.GetAllUsers());
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _service.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _service.AddUser(user);

            return Ok("User Added Successfully");
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (id != user.UserId)
                return BadRequest();

            _service.UpdateUser(user);

            return Ok("User Updated Successfully");
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _service.DeleteUser(id);

            return Ok("User Deleted Successfully");
        }
    }
}