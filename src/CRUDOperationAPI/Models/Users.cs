using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.Models
{
    public class Users : IdentityUser
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public virtual Roles Role { get; set; }

        
    }
}
