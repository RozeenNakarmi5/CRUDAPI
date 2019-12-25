using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.InterfaceClass
{
    interface IUserService
    {
        List<object> GetUserProfile();
    }
}
