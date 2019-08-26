using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer
{
    public class StudentPersonalInfo
    {
        public string StudentId { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CitizenshipNumber { get; set; }
        public string Email { get; set; }
        //public string Semester { get; set; } Removed since we can get from Student Table
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string GuardianName { get; set; }
        public string GuardianRelationship { get; set; }
        public string GuardianContactNumber { get; set; } //Added later
        public DateTime? InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime AdmissionDate { get; set; }

        public virtual Student Student { get; set; }
    }
}
