using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.ViewModels
{
    public class ViewClientAndProject
    {
        public int ClientProjectID { get; set; }
        public int ProjectID { get; set; }
        public int ClientID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string ClientFirstName { get; set; }
        public bool IsActive { get; set; }
    }
}
