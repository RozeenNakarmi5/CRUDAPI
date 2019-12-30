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
    public class ProjectController : Controller
    {
        private IProjectService _project;
        public ProjectController(IProjectService project)
        {
            _project = project;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<ProjectViewModel> GetNow()
        {
            var getProject = _project.GetNow();
            return getProject;
        }

        [Route("GetScrap")]
        [HttpGet]
        public IEnumerable<ProjectViewModel> GetScrap()
        {
            var getProject = _project.GetScrap();
            return getProject;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjects(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var projects = _project.GetProjectByID(id);
            if (projects == null)
            {
                return NotFound();
            }
            return Ok(projects);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> PostProjects([FromBody]ProjectViewModel projects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _project.PostProject(projects);
            return CreatedAtAction("GetProjects", new { id = projects.ProjectID }, projects);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjects(int id, [FromBody]ProjectViewModel projects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (id != projects.ProjectID)
            //{
            //    return BadRequest();
            //}
            _project.PutProject(projects);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProjects(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var projects = _project.DisableProject(id);
            if (projects == 0)
            {
                return NotFound();
            }
            return Ok(projects);
        }

        [Route("GetScrap/{id}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> RestoreProjects(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var projects = _project.EnableProject(id);
            if (projects == 0)
            {
                return NotFound();
            }
            return Ok(projects);
        }

        [Route("ProjectCount")]
        public IActionResult ProjectCount()
        {
            var countProject = _project.CountProject();
            return Ok(countProject);
        }
    }
}
