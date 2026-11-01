using System.ComponentModel.DataAnnotations;
using CoreLayer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Models;

public class CourseViewModel
{
    public int CourseId { get; set; }

    [Required, StringLength(150)]
    [Display(Name = "Course name")]
    public string CourseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Choose a department.")]
    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }

    [Required(ErrorMessage = "Choose a level.")]
    [Display(Name = "Level")]
    public int? LevelId { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Duration { get; set; }

    [Range(0, 12)]
    [Display(Name = "Credit hours")]
    public int Credit { get; set; } = 3;

    [ValidateNever]
    public IEnumerable<SelectListItem> Departments { get; set; } = [];

    [ValidateNever]
    public IEnumerable<SelectListItem> Levels { get; set; } = [];

    public static CourseViewModel From(Course c) => new()
    {
        CourseId = c.CourseId,
        CourseName = c.CourseName,
        DepartmentId = c.DepartmentId,
        LevelId = c.LevelId,
        Description = c.Description,
        Duration = c.Duration,
        Credit = c.Credit,
    };

    public Course ToEntity() => new()
    {
        CourseId = CourseId,
        CourseName = CourseName.Trim(),
        DepartmentId = DepartmentId!.Value,
        LevelId = LevelId!.Value,
        Description = Description,
        Duration = Duration,
        Credit = Credit,
    };
}
