using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
