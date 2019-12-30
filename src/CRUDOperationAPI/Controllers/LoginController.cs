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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDOperationAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginServices _login;

        public LoginController(ILoginServices login)
        {
            _login = login;
        }
        [HttpPost]
        public string Login([FromBody] Users Login)
        {

            return _login.GenerateToken(Login);

        }
        [Authorize(Roles = "55")]
        [HttpGet]
        public int number()
        {
            return 0;
        }
    }
}
