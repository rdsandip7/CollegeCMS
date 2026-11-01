using System.ComponentModel.DataAnnotations;
using CoreLayer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Models;

public class StudentViewModel : IValidatableObject
{
    public string? StudentId { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50)]
    [Display(Name = "Middle name")]
    public string? MiddleName { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Choose a level.")]
    [Display(Name = "Level")]
    public int? LevelId { get; set; }

    [Required(ErrorMessage = "Choose a department.")]
    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }

    [Phone, StringLength(20)]
    [Display(Name = "Contact number")]
    public string? Contact { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(100)]
    [Display(Name = "Guardian name")]
    public string? GuardianName { get; set; }

    [Phone, StringLength(20)]
    [Display(Name = "Guardian contact")]
    public string? GuardianContact { get; set; }

    [StringLength(50)]
    [Display(Name = "Relationship")]
    public string? GuardianRelationship { get; set; }

    public string? Gender { get; set; }

    [EmailAddress, StringLength(200)]
    public string? Email { get; set; }

    [StringLength(30)]
    [Display(Name = "Citizenship number")]
    public string? CitizenshipNumber { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of birth")]
    public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-18);

    [DataType(DataType.Date)]
    [Display(Name = "Admission date")]
    public DateTime AdmissionDate { get; set; } = DateTime.Today;

    [ValidateNever]
    public IEnumerable<SelectListItem> Departments { get; set; } = [];

    [ValidateNever]
    public IEnumerable<SelectListItem> Levels { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateOfBirth >= DateTime.Today)
            yield return new ValidationResult("Date of birth must be in the past.", [nameof(DateOfBirth)]);
        if (AdmissionDate <= DateOfBirth)
            yield return new ValidationResult("Admission date must be after the date of birth.", [nameof(AdmissionDate)]);
    }

    public static StudentViewModel From(Student s) => new()
    {
        StudentId = s.StudentId,
        FirstName = s.FirstName,
        MiddleName = s.MiddleName,
        LastName = s.LastName,
        LevelId = s.LevelId,
        DepartmentId = s.DepartmentId,
        Contact = s.PersonalInfo.ContactNumber,
        Address = s.PersonalInfo.Address,
        GuardianName = s.PersonalInfo.GuardianName,
        GuardianContact = s.PersonalInfo.GuardianContactNumber,
        GuardianRelationship = s.PersonalInfo.GuardianRelationship,
        Gender = s.PersonalInfo.Gender,
        Email = s.PersonalInfo.Email,
        CitizenshipNumber = s.PersonalInfo.CitizenshipNumber,
        DateOfBirth = s.PersonalInfo.DateOfBirth,
        AdmissionDate = s.PersonalInfo.AdmissionDate,
    };

    public Student ToEntity() => new()
    {
        StudentId = StudentId ?? string.Empty,
        FirstName = FirstName.Trim(),
        MiddleName = MiddleName?.Trim(),
        LastName = LastName.Trim(),
        LevelId = LevelId!.Value,
        DepartmentId = DepartmentId!.Value,
        PersonalInfo = new StudentPersonalInfo
        {
            StudentId = StudentId ?? string.Empty,
            ContactNumber = Contact,
            Address = Address,
            GuardianName = GuardianName,
            GuardianContactNumber = GuardianContact,
            GuardianRelationship = GuardianRelationship,
            Gender = Gender,
            Email = Email,
            CitizenshipNumber = CitizenshipNumber,
            DateOfBirth = DateOfBirth,
            AdmissionDate = AdmissionDate,
        },
    };
}
