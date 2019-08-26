using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class Department
    {
        public Department()
        {
            Employees = new List<Employee>();
            Students = new List<Student>();
            Courses = new List<Course>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public string DepartmentHead { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
