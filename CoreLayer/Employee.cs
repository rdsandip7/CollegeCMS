namespace CoreLayer;

public class Employee
{
    public int EmployeeId { get; set; }
    public int? UserId { get; set; }
    public int DepartmentId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Department? Department { get; set; }
    public EmployeePersonalInfo PersonalInfo { get; set; } = new();

    public string FullName => string.Join(" ", new[] { FirstName, MiddleName, LastName }.Where(n => !string.IsNullOrWhiteSpace(n)));
}
