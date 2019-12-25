using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using CRUDOperationAPI.Models;
using Microsoft.AspNetCore.Http;

namespace CRUDOperationAPI.InterfaceClass
{
    public interface ILoginServices
    {
        string GenerateToken(Users Login);
        int Logout(int UserID);
    }
}
