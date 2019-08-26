using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class Student
    {
        public string StudentId { get; set; }
        public int DepartmentId { get; set; }
        public int LevelId { get; set; }
        public string FirstName { get; set; } //added later
        public string MiddleName { get; set; } //added later
        public string LastName { get; set; } //added later
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Department Department { get; set; }
        public virtual StudentPersonalInfo StudentPersonalInfo { get; set; }
        public virtual Level Level { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
