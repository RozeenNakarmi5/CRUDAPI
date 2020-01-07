using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDOperationAPI.InterfaceClass;
using CRUDOperationAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDOperationAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private IClientService _client;
        public ClientController(IClientService client)
        {
            _client = client;
        }

        
        [HttpGet]
        public IEnumerable<ClientProjectViewModel> Get()
        {
            var getAllClient = _client.GetALL();
            return getAllClient;
        }

        
        // GET api/values/5
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClients(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var clients = _client.GetClientByID(id);
            if (clients == null)
            {
                return NotFound();
            }
            return Ok(clients);

        }

        // POST api/values
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostClients([FromBody]ClientProjectViewModel clients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _client.PostClient(clients);
            return CreatedAtAction("GetClients", new { id = clients.ClientID }, clients);
        }

        // PUT api/values/5
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutClients(int id, [FromBody]ClientProjectViewModel clients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != clients.ClientID)
            {
                return BadRequest();
            }
            _client.PutClient(clients);
            return Ok();
        }

        // DELETE api/values/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClients(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var clients = _client.DeleteClient(id);
            if(clients == 0)
            {
                return NotFound();
            }
            return Ok(clients);
        }
        [Route("ClientCount")]
        [HttpGet]
        public IActionResult ProjectCount()
        {
            var countClient = _client.CountClient();
            return Ok(countClient);
        }
        [Route("AssignProjectToClient")]
        [Authorize(Roles = "Team Leads")]
        [HttpPost]
        public async Task<IActionResult> AssignProjectToClient([FromBody]ClientProjectViewModel clients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _client.AssignProjectToClient(clients);
            return Ok();
        }

        // GET: api/values
        [Route("SetClient")]
        [HttpGet]
        public IEnumerable<ViewClientAndProject> GetClientAndProjectDetail()
        {
            var getAllClientProject = _client.GetClientProject();
            return getAllClientProject;
        }

        [Route("SetClient/{id}")]
        [Authorize(Roles = "Team Leads")]
        [HttpPut]
        public async Task<IActionResult> UpdateClientProject(int id, [FromBody]ClientProjectViewModel clients)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != clients.ClientProjectID)
            {
                return BadRequest();
            }
            _client.UpdateClientProject(clients);
            return Ok();
        }
        [Route("SetClient/{id}")]
        [Authorize(Roles = "Team Leads")]
        [HttpDelete]
        public async Task <IActionResult> DeleteClientProject(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var clients = _client.DeleteClientProject(id);
            if (clients == 0)
            {
                return NotFound();
            }
            return Ok(clients);
        }

    }
}
