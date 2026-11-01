using System.ComponentModel.DataAnnotations;
using CoreLayer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Models;

public class EmployeeViewModel : IValidatableObject
{
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Choose a department.")]
    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50)]
    [Display(Name = "Middle name")]
    public string? MiddleName { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Please enter an email address.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Position { get; set; }

    public string? Gender { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Hired date")]
    public DateTime HiredDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Resignation date")]
    public DateTime? ResignationDate { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> Departments { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ResignationDate is not null && ResignationDate < HiredDate)
            yield return new ValidationResult("Resignation date cannot be before the hired date.", [nameof(ResignationDate)]);
    }

    public static EmployeeViewModel From(Employee e) => new()
    {
        EmployeeId = e.EmployeeId,
        DepartmentId = e.DepartmentId,
        FirstName = e.FirstName,
        MiddleName = e.MiddleName,
        LastName = e.LastName,
        Email = e.Email,
        Phone = e.PersonalInfo.Phone,
        Address = e.PersonalInfo.Address,
        Position = e.PersonalInfo.Position,
        Gender = e.PersonalInfo.Gender,
        HiredDate = e.PersonalInfo.HiredDate,
        ResignationDate = e.PersonalInfo.ResignationDate,
    };

    public Employee ToEntity() => new()
    {
        EmployeeId = EmployeeId,
        DepartmentId = DepartmentId!.Value,
        FirstName = FirstName.Trim(),
        MiddleName = MiddleName?.Trim(),
        LastName = LastName.Trim(),
        Email = Email.Trim(),
        PersonalInfo = new EmployeePersonalInfo
        {
            EmployeeId = EmployeeId,
            Phone = Phone,
            Address = Address,
            Position = Position,
            Gender = Gender,
            HiredDate = HiredDate,
            ResignationDate = ResignationDate,
        },
    };
}
