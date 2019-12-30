using CRUDOperationAPI.Contexts;
using CRUDOperationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly EmployeeDbContext _database;


        public UserController(EmployeeDbContext database)
        {
            _database = database;
        }
        [HttpGet]
        [Authorize]
        [Route("UserDetail")]
        public async Task<object> Get()
        {
            try
            {
                int UserID = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
                var emp = _database.Employees.FirstOrDefault(x => x.UserID == UserID);
                var contact = _database.Contacts.FirstOrDefault(x => x.ContactID == emp.ContactId);
                return new
                {
                    contact.FirstName,
                    contact.LastName
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
