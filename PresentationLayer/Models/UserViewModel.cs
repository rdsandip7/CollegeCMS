using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace PresentationLayer.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage="Please Enter First Name")]
        [Display(Name="First Name")]
        [StringLength(50, ErrorMessage="Please Enter The Name Less Than 50 Character")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Please Enter The Name Less Than 50 Character")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Please Enter The Name Less Than 50 Character")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter User Name")]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage="Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Provide Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage="Confirm Password doesn't match with Password Please provide same password")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}