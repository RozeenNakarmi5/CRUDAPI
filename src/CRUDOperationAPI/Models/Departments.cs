using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.Models
{
    public class Departments
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public virtual int CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        public Departments()
        {
            CompanyID = 1;   
        }

    }
}
