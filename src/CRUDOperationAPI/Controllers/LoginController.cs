using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDOperationAPI.Contexts;
using CRUDOperationAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using CRUDOperationAPI.InterfaceClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDOperationAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        private readonly ILoginServices _login;
        private readonly EmployeeDbContext _database;


        public LoginController(ILoginServices login, EmployeeDbContext database)
        {
            _login = login;
            _database = database;
        }
        [HttpPost]
        public IActionResult Login([FromBody] Users Login)
        {
            var token = _login.GenerateToken(Login);
            if (token == "Username or password is incorrect")
            {
                return NotFound("Invalid username or password");
            }
            else
            {
                return Ok(new { token });
            }
            

        }

        [Authorize]
        [Route("Logout")]
        [HttpPut]
        public void Logout()
        {
            try
            {
                int UserID = Convert.ToInt32(User.Claims.First(c => c.Type == "UserID").Value);
                _login.Logout(UserID);
            }
            catch
            {
                 BadRequest(new { message = "User is not logged in." });
            }
        }
    }
}
