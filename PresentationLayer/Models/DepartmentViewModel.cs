using System.ComponentModel.DataAnnotations;
using CoreLayer;

namespace PresentationLayer.Models;

public class DepartmentViewModel
{
    public int DeptId { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Department name")]
    public string DepartmentName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Established date")]
    public DateTime? EstablishedDate { get; set; }

    [StringLength(100)]
    [Display(Name = "Department head")]
    public string? DepartmentHead { get; set; }

    public static DepartmentViewModel From(Department d) => new()
    {
        DeptId = d.DepartmentId,
        DepartmentName = d.DepartmentName,
        Description = d.Description,
        EstablishedDate = d.EstablishedDate,
        DepartmentHead = d.DepartmentHead,
    };

    public Department ToEntity() => new()
    {
        DepartmentId = DeptId,
        DepartmentName = DepartmentName.Trim(),
        Description = Description,
        EstablishedDate = EstablishedDate,
        DepartmentHead = DepartmentHead,
    };
}
