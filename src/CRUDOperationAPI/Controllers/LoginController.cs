using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDOperationAPI.Contexts;
using Microsoft.Extensions.Options;
using CRUDOperationAPI.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDOperationAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly EmployeeDbContext _Context;
        private readonly IOptions<TokenAuthentication> _auth;

        public LoginController(IOptions<TokenAuthentication> auth, EmployeeDbContext context)
        {
            _Context = context;
            _auth = auth;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel LoginUser)
        {

            //public async Task<IActionResult> LogIn([FromBody] LoginModel LoginUser)
            //{
            try
            {
                //var user = await _userManager.FindByNameAsync(LoginUser.UserName);

                var query = (from t in _Context.Users where LoginUser.UserName == t.UserName && LoginUser.Password == t.Password select t).FirstOrDefault();
                if (query != null)
                {
                    //if (user != null && await _userManager.CheckPasswordAsync(user, LoginUser.Password))
                    //{
                    //var role = await _userManager.GetRolesAsync(user);
                    //IdentityOptions _options = new IdentityOptions();

                    //clean to use from appsettings
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.Value.SecretKey));
                    var code = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, LoginUser.UserName)
                        //, new Claim(_options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                    };


                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: _auth.Value.Issuer,
                        audience: _auth.Value.Audience,
                        expires: DateTime.UtcNow.AddDays(1),
                        claims: claims,
                        signingCredentials: code
                        );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        expiration = jwtSecurityToken.ValidTo
                    });

                }
                else
                {
                    return BadRequest(new { message = "Username or password is incorrect" });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public int number()
        {
            return 0;
        }



    }
}
