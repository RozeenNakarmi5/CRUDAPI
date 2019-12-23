﻿using CRUDOperationAPI.Contexts;
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
                         where Login.UserName == t.UserName && Login.Password == t.Password select new {login.RoleName,t.UserName,t.Password }).FirstOrDefault();
            if (query != null)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.Value.SecretKey));
                var code = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescriptions = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserName", query.UserName),
                        new Claim("Password", query.Password),
                        new Claim(ClaimTypes.Role, query.RoleName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = code
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptions);
                var tokenString = tokenHandler.WriteToken(securityToken);
                return tokenString; 
            }
            else
                return "Username or password is incorrect";
        }
        
    }
}
