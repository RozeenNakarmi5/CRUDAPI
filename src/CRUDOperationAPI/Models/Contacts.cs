  using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.Models
{
    public class Contacts
    {
        [Key]
        public int ContactID { get; set; }
        [Required(ErrorMessage = "Please enter first name of the employee")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter last name of the employee")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter address of the employee")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter Email of the employee")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter contact number of the employee")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Please enter emergency contact number of the employee")]
        public string EmergencyContactNumber { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime ModifiedTimeStamp { get; set; }
    }
}
