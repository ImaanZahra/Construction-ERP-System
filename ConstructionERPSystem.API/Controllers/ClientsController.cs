using ConstructionERPSystem.API.Models;
using ConstructionERPSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientsController(IClientService service)
        {
            _service = service;
        }

        // GET: api/clients
        [HttpGet]
        public IActionResult GetAllClients()
        {
            return Ok(_service.GetAllClients());
        }

        // GET: api/clients/5
        [HttpGet("{id}")]
        public IActionResult GetClientById(int id)
        {
            var client = _service.GetClientById(id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        public IActionResult AddClient(Client client)
        {
            _service.AddClient(client);

            return Ok("Client Added Successfully");
        }

        // PUT: api/clients/5
        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, Client client)
        {
            if (id != client.ClientId)
                return BadRequest();

            _service.UpdateClient(client);

            return Ok("Client Updated Successfully");
        }

        // DELETE: api/clients/5
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            _service.DeleteClient(id);

            return Ok("Client Deleted Successfully");
        }
    }
}