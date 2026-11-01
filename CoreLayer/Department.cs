namespace CoreLayer;

public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DepartmentHead { get; set; }
    public DateTime? EstablishedDate { get; set; }
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
