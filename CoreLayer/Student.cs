namespace CoreLayer;

public class Student
{
    public string StudentId { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int LevelId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Department? Department { get; set; }
    public Level? Level { get; set; }
    public StudentPersonalInfo PersonalInfo { get; set; } = new();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public string FullName => string.Join(" ", new[] { FirstName, MiddleName, LastName }.Where(n => !string.IsNullOrWhiteSpace(n)));
}
