namespace CoreLayer;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public DateTime EnrolledDate { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
