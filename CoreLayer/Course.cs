using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class Course
    {
        public Course()
        {
            Enrollments = new List<Enrollment>();
        }

        public int CourseId { get; set; }
        public int DepartmentId { get; set; }
        public int LevelId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string Credit { get; set; }
        public string Duration { get; set; }
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Department Departments { get; set; }
        public virtual Level Levels { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
