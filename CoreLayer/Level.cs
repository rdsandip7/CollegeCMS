using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class Level
    {
        public Level()
        {
            Students = new List<Student>();
            Courses = new List<Course>();
        }

        public int LevelId { get; set; }
        public DateTime Year { get; set; }
        public string Semester { get; set; }
        public string Description { get; set; }
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
