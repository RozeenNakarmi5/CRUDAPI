using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDOperationAPI.Models;
using CRUDOperationAPI.InterfaceClass;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDOperationAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<Departments> Get()
        {
            return _departmentService.GetAll();
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Departments GetByID(int id)
        {
            return _departmentService.GetDepartmentByID(id);
        }

        // POST api/values
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] Departments dept)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _departmentService.PostDepartment(dept);
                return CreatedAtAction("GetByID", new { id = dept.DepartmentID }, dept);
            }
            catch 
            {
               return BadRequest(new { message = "Department Name alredy exists" });
            }
        }

        // PUT api/values/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody]Departments dept)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != dept.DepartmentID)
            {
                return BadRequest();
            }
            try
            {
                _departmentService.PutDepartment(dept);
                return Ok();
            }
            catch
            {
                return BadRequest(new { message = "Username alredy exists" });

            }

        }

        // DELETE api/values/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var deleteDepartment = _departmentService.DeleteDepartment(id);
          
            return Ok(deleteDepartment);
        }

        [Route("DepartmentCount")]
        [HttpGet]
        public IActionResult DepartmentCount()
        {
            var countDepartment = _departmentService.CountDepartment();
            return Ok(countDepartment);
        }
    }
}
