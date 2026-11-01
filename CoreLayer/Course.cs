namespace CoreLayer;

public class Course
{
    public int CourseId { get; set; }
    public int DepartmentId { get; set; }
    public int LevelId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credit { get; set; }
    public string? Duration { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Department? Department { get; set; }
    public Level? Level { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
