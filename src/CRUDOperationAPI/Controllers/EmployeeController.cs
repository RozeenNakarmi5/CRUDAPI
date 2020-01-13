using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDOperationAPI.Contexts;
using CRUDOperationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using CRUDOperationAPI.Connections;
using CRUDOperationAPI.Implementation;
using CRUDOperationAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using CRUDOperationAPI.PaginationClass;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDOperationAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        
        private IEmployeeService _employee;

        public EmployeeController(IEmployeeService employee)
        {
            _employee = employee;
        //    //_context = context;
        //    //var connection = connectionConfig.Value;
        //    //string connectionString = connection.myconn;
        //    //_employee = new EmployeeImplementation(connectionString);


        }
        // GET: api/values
        [HttpGet]
        public IActionResult GetWorkingEmployee([FromQuery] Pagination pagination)
        {
            //return _context.Employees;
            //using (SqlConnection connection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=EmployeeDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
            //{
            //    var eventName = connection.QueryFirst<Employee>("SELECT * FROM Employees");
            //    yield return eventName;
            //}
            var getAllEmployee =  _employee.GetWorkingEmployee(pagination);
            return Ok(getAllEmployee);

        }

        [Route("GetNotWorkingEmployee")]
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IEnumerable<EmployeeContactsRole> GetNotWorkingEmployee()
        {
            var getAllEmployee = _employee.GetNotWorkingEmployee();
            return getAllEmployee;
        }


        //GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmplooyes(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employees = _employee.GetEmployeeByID(id);
            if (employees == null)
            {
                return NotFound();
            }
               return Ok(employees);
            }

        // POST api/values
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> PostEmployees([FromBody] EmployeeContactsRole employees)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _employee.PostEmployee(employees);

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        // PUT api/values/5
        [Authorize(Roles = "Admin, Team Leads")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees([FromRoute] int id, [FromBody] EmployeeContactsRole employees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employees.EmployeeID)
            {
                return BadRequest();
            }

            _employee.PutEmployee(employees);



            return Ok();
        }
        //// DELETE api/values/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employees = _employee.RemoveEmployee(id);
            //var employees = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employees == 0)
            {
                return NotFound();
            }

            //_context.Employees.Remove(employees);
            //await _context.SaveChangesAsync();

            return Ok(employees);
        }

        [Route("EmployeeCount")]
        [HttpGet]
        public IActionResult EmployeeCount()
        {
            var countEmployee = _employee.CountEmployee();
            return Ok(countEmployee);
        }

        [Route("AddUsers/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateUsers([FromRoute] int id, [FromBody] UsersDetail employees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employees.EmployeeId)
            {
                return BadRequest();
            }
            _employee.AddUsers(employees);
            return Ok();
        }

        [Route("UpdateRole/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromRoute] int id, [FromBody] UpdateRole roles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roles.EmployeeId)
            {
                return BadRequest();
            }
            try
            {
                _employee.UpdateRoleOfEmployee(roles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();

        }
        [Route("UpdateDepartment/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] EmployeeDepartmentViewModel departments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != departments.EmployeeId)
            {
                return BadRequest();
            }
            try
            {
                _employee.UpdateEmployeeDepartment(departments);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("UpdateContact/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] EmployeeContactsRole contacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contacts.ContactID)
            {
                return BadRequest();
            }
            try
            {
                _employee.UpdateContact(contacts);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("AssignProjectToEmployee")]
        [Authorize(Roles = "Admin, Team Leads")]
        [HttpPost]
        public IActionResult AssignProjectToEmployee([FromBody]EmployeeProjectViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _employee.AssignProjectToEmployee(employee);
            return Ok();
        }

        [Route("ExportEmpSchedule")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public string ExportEmpSchedule()
        {
            return _employee.ExportEmployeeSchedule();
        }
        [Route("GetEmpSchedule")]
        [HttpGet]
        public IActionResult GetEmpSchedule([FromQuery] Pagination pagination)
        {
            var getEmpSchedule = _employee.GetEmployeeSchedule(pagination);
            return Ok(getEmpSchedule);
        }
        [Route("CountEmpSchedule")]
        [HttpGet]
        public IActionResult CountEmpSchedule()
        {
            var count = _employee.CountEmpSchedule();
            return Ok(count);
        }
        [AllowAnonymous]
        [Route("GetWorkTime")]
        [HttpGet]
        public IActionResult GetWorkingHrs()
        {
            var getTime= _employee.GetTotalWorkingHrs();
            return Ok(getTime);
        }

    }

}
