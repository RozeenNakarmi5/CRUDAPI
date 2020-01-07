using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.ViewModels
{
    public class EmployeeContactsRole
    {
        public int? EmployeeID { get; set; }
        public int? ContactID { get; set; }
        public int? RoleID { get; set; }
        public int? UserID { get; set; }
        public int? DepartmentID { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public decimal? Salary { get; set; }
        public bool? IsFullTimer { get; set; }
        public bool? IsWorking { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DepartmentName { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime ModifiedTimeStamp { get; set; }
        public EmployeeContactsRole()
        {
            CreatedTimeStamp = DateTime.Now;
            ModifiedTimeStamp = DateTime.Now;
            IsWorking = true;
        }
    }
}
