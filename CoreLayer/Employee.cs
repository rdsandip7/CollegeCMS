using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int? UserId { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; } //added later
        public string MiddleName { get; set; } //added later
        public string LastName { get; set; } //added later
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Department Department { get; set; }
        public virtual EmployeePersonalInfo EmployeePersonalInfo { get; set; }
    }
}
