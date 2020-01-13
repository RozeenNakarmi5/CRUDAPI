using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.ViewModels
{
    public class EmployeeDepartmentWorkTime
    {
        public int? EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string DepartmentName { get; set; }
        public int? totalhrs { get; set; }
        public int? avghrs { get; set; }
    }
}
