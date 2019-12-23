using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.ViewModels
{
    public class EmployeeProjectViewModel
    {
        public int EmployeeProjectID { get; set; }
        public List<int> ProjectID { get; set; }
        public int EmployeeID { get; set; }
        public string ProjectName { get; set; }
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string EmergencyContactNumber { get; set; }
    }
}
