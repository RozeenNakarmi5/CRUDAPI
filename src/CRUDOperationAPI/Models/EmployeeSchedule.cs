using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.Models
{
    public class EmployeeSchedule
    {
        [Key]
        public int ScheduleID { get; set; }
        public virtual int EmployeeID { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public TimeSpan TotalHourWorkPerday { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime ModifiedTimeStamp { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public EmployeeSchedule()
        {
            CreatedTimeStamp = DateTime.Now;
        }
    }
}
