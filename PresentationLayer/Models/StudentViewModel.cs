using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class StudentViewModel
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Level { get; set; }
        public string Department { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string GuardianName { get; set; }
        public string GuardianContact { get; set; }
        public string GuardianRelationship { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string CitizenshipNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime AdmissionDate { get; set; }
    }
}