using CRUDOperationAPI.Contexts;
using CRUDOperationAPI.InterfaceClass;
using CRUDOperationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRUDOperationAPI.Implementation
{
    public class UserProfileImplementation 
    {
        private readonly EmployeeDbContext _database;
        public UserProfileImplementation(EmployeeDbContext database)
        {
            _database = database;
        }
        //public List<object> GetUserProfile()
        //{
        //    ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;

        //}
    }
}
