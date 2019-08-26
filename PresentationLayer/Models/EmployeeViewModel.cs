using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string Department { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        public string Position { get; set; }
        public string Gender { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime? ResignationDate { get; set; }

    }
}