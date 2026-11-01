namespace CoreLayer;

public class StudentPersonalInfo
{
    public string StudentId { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? CitizenshipNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ContactNumber { get; set; }
    public string? GuardianName { get; set; }
    public string? GuardianRelationship { get; set; }
    public string? GuardianContactNumber { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Student? Student { get; set; }
}
