namespace CoreLayer;

public class Level
{
    public int LevelId { get; set; }
    public int Year { get; set; }
    public string Semester { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();

    /// <summary>Year is the programme year (1-4), so the semester name alone identifies the level.</summary>
    public string DisplayName => Semester;
}
