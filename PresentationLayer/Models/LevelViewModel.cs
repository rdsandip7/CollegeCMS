using System.ComponentModel.DataAnnotations;
using CoreLayer;

namespace PresentationLayer.Models;

public class LevelViewModel
{
    public int LevelId { get; set; }

    [Required, Range(2000, 2100, ErrorMessage = "Enter a year between 2000 and 2100.")]
    public int Year { get; set; } = DateTime.Today.Year;

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
