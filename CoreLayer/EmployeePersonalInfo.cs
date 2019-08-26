using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class EmployeePersonalInfo
    {
        public int EmployeeId { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }        
        public string Phone { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime? ResignationDate { get; set; }
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
