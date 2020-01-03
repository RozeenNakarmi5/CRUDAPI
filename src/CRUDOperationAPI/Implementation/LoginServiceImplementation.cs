using CRUDOperationAPI.Contexts;
using CRUDOperationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CRUDOperationAPI.InterfaceClass;
using System.Security.Claims;
using System.Data;
using CRUDOperationAPI.Connections;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace CRUDOperationAPI.Implementation
{
    public class UserServiceImplementation : ILoginServices
    {
        private readonly EmployeeDbContext _Context;
        private readonly IOptions<TokenAuthentication> _auth;


        public UserServiceImplementation(EmployeeDbContext context, IOptions<TokenAuthentication> auth)
        {
            _Context = context;
            _auth = auth;
        }


        public string GenerateToken(Users Login)
        {
            var query = (from t in _Context.Users
                         join login in _Context.Roles on t.RoleID equals login.RoleID
                         where Login.UserName == t.UserName && Login.Password == t.Password select new {login.RoleName,t.UserName,t.Password,t.UserID }).FirstOrDefault();
            var getEmployee = (from users in _Context.Users
                               join emp in _Context.Employees on users.UserID equals emp.UserID
                               where Login.UserName == users.UserName
      
                               select new { emp.EmployeeId }).FirstOrDefault()
                               ;
            if (query != null)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.Value.SecretKey));
                var code = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescriptions = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", query.UserID.ToString()),
                        new Claim(ClaimTypes.Role, query.RoleName)
                    }),
                    Issuer = _auth.Value.Issuer,
                    Audience = _auth.Value.Audience,
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = code
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptions);
                var tokenString = tokenHandler.WriteToken(securityToken);

                try
                {
                    var empSchedule = _Context.EmployeeSchedule.Where(x => x.EmployeeID == getEmployee.EmployeeId).OrderByDescending(x => x.CreatedTimeStamp).FirstOrDefault();                    
                        if (empSchedule == null)
                        {
                            var insertIntoEmpSchedule = new EmployeeSchedule
                            {
                                EmployeeID = getEmployee.EmployeeId,
                                InTime = DateTime.Now
                            };
                            _Context.EmployeeSchedule.Add(insertIntoEmpSchedule);
                            _Context.SaveChanges();
                        }
                        else if (empSchedule != null)
                        {
                            var insertAnother = new EmployeeSchedule
                            {
                                EmployeeID = getEmployee.EmployeeId,
                                InTime = DateTime.Now
                            };
                            _Context.EmployeeSchedule.Add(insertAnother);
                            if(empSchedule.OutTime != null)
                            {
                                _Context.SaveChanges();
                            }

                        return tokenString;  
                        }
                    
                    
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
                return tokenString; 
            }
            else
            {
                return "Username or password is incorrect";

            }
        }

        public int Logout(int UserID)
        {

            var emp = (from empz in _Context.Employees
                       where empz.UserID == UserID
                       select empz).FirstOrDefault();
            //var empSchedule = _database.EmployeeSchedule.Where(x => x.EmployeeID == emp.EmployeeId && x.OutTime == null).Select(x => x.ScheduleID);
            var empSchedule = (from empSc in _Context.EmployeeSchedule
                               where empSc.EmployeeID == emp.EmployeeId && empSc.OutTime == null
                               select empSc).OrderByDescending(x => x.CreatedTimeStamp).FirstOrDefault();
            empSchedule.OutTime = DateTime.Now;
            empSchedule.TotalHourWorkPerday = empSchedule.OutTime.Value.Subtract(empSchedule.InTime.Value);
            empSchedule.ModifiedTimeStamp = DateTime.Now;
            _Context.EmployeeSchedule.Update(empSchedule);
            return _Context.SaveChanges();
        }
    }
}
