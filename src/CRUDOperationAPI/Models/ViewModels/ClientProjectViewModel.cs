using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.ViewModels
{
    public class ClientProjectViewModel
    {
        public int ClientProjectID { get; set; }
        public List<int> ProjectID { get; set; }
        public int ClientID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientOffice { get; set; }
        public string OfficeAddress { get; set; }
        public string ClientContactNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime ModifiedTimeStamp { get; set; }
        public ClientProjectViewModel()
        {
            IsActive = true;
            CreatedTimeStamp = DateTime.Now;
            ModifiedTimeStamp = DateTime.Now;
        }

    }
}
