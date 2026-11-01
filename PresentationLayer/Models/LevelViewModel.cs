using System.ComponentModel.DataAnnotations;
using CoreLayer;

namespace PresentationLayer.Models;

public class LevelViewModel
{
    public int LevelId { get; set; }

    [Required, Range(1, 6, ErrorMessage = "Enter a programme year between 1 and 6.")]
    [Display(Name = "Programme year")]
    public int Year { get; set; } = 1;

    [Required, StringLength(50)]
    public string Semester { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    public static LevelViewModel From(Level l) => new()
    {
        LevelId = l.LevelId,
        Year = l.Year,
        Semester = l.Semester,
        Description = l.Description,
    };

    public Level ToEntity() => new()
    {
        LevelId = LevelId,
        Year = Year,
        Semester = Semester.Trim(),
        Description = Description,
    };
}
