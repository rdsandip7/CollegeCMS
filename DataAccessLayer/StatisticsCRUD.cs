using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public record CollegeStatistics(
    int Students,
    int NewAdmissions,
    int Faculty,
    int Staff,
    int Courses,
    int Programmes,
    int Departments,
    int Enrollments,
    int FemaleStudents);

public record DepartmentSummary(int DepartmentId, string Department, string? Head, string? Description, DateTime? Established, int Students, int Faculty, int Staff, int Courses);

public record IntakeCount(int Year, string Department, int Students);

public record PositionCount(string Position, int Count);

public record RecentStudent(string StudentId, string Name, string Department, string Level, DateTime AdmissionDate);

public record RecentHire(int EmployeeId, string Name, string Department, string? Position, DateTime HiredDate);

public class StatisticsCRUD(DataContext dataContext)
{
    private IQueryable<Employee> ActiveEmployees => dataContext.Employees.Where(e => e.PersonalInfo.ResignationDate == null);

    public async Task<CollegeStatistics> GetStatisticsAsync()
    {
        var latestIntake = await dataContext.StudentPersonalInfos.MaxAsync(p => (DateTime?)p.AdmissionDate);
        var intakeYear = latestIntake?.Year ?? DateTime.Today.Year;

        return new CollegeStatistics(
            Students: await dataContext.Students.CountAsync(),
            NewAdmissions: await dataContext.StudentPersonalInfos.CountAsync(p => p.AdmissionDate.Year == intakeYear),
            Faculty: await ActiveEmployees.CountAsync(e => StaffPositions.Teaching.Contains(e.PersonalInfo.Position!)),
            Staff: await ActiveEmployees.CountAsync(),
            Courses: await dataContext.Courses.CountAsync(),
            Programmes: await dataContext.Departments.CountAsync(d => d.Students.Any()),
            Departments: await dataContext.Departments.CountAsync(),
            Enrollments: await dataContext.Enrollments.CountAsync(),
            FemaleStudents: await dataContext.StudentPersonalInfos.CountAsync(p => p.Gender == "Female"));
    }

    public Task<List<DepartmentSummary>> GetDepartmentSummariesAsync() =>
        dataContext.Departments
            .OrderByDescending(d => d.Students.Count)
            .ThenBy(d => d.DepartmentName)
            .Select(d => new DepartmentSummary(
                d.DepartmentId,
                d.DepartmentName,
                d.DepartmentHead,
                d.Description,
                d.EstablishedDate,
                d.Students.Count,
                d.Employees.Count(e => e.PersonalInfo.ResignationDate == null && StaffPositions.Teaching.Contains(e.PersonalInfo.Position!)),
                d.Employees.Count(e => e.PersonalInfo.ResignationDate == null),
                d.Courses.Count))
            .ToListAsync();

    /// <summary>Current students grouped by intake (admission) year and department.</summary>
    public async Task<List<IntakeCount>> GetIntakeCountsAsync()
    {
        var rows = await dataContext.Students
            .GroupBy(s => new { s.PersonalInfo.AdmissionDate.Year, s.Department!.DepartmentName })
            .Select(g => new { g.Key.Year, g.Key.DepartmentName, Count = g.Count() })
            .ToListAsync();
        return rows.Select(r => new IntakeCount(r.Year, r.DepartmentName, r.Count))
            .OrderBy(r => r.Year).ThenBy(r => r.Department).ToList();
    }

    public async Task<List<PositionCount>> GetStaffByPositionAsync()
    {
        var rows = await ActiveEmployees
            .GroupBy(e => e.PersonalInfo.Position)
            .Select(g => new { Position = g.Key, Count = g.Count() })
            .ToListAsync();
        return rows.Select(r => new PositionCount(r.Position ?? "Unassigned", r.Count))
            .OrderByDescending(r => r.Count).ThenBy(r => r.Position).ToList();
    }

    public Task<List<RecentStudent>> GetRecentAdmissionsAsync(int count) =>
        dataContext.Students
            .OrderByDescending(s => s.InsertedDate)
            .ThenByDescending(s => s.StudentId)
            .Take(count)
            .Select(s => new RecentStudent(
                s.StudentId,
                s.FirstName + " " + s.LastName,
                s.Department!.DepartmentName,
                s.Level!.Semester,
                s.PersonalInfo.AdmissionDate))
            .ToListAsync();

    public Task<List<RecentHire>> GetRecentHiresAsync(int count) =>
        ActiveEmployees
            .OrderByDescending(e => e.PersonalInfo.HiredDate)
            .Take(count)
            .Select(e => new RecentHire(
                e.EmployeeId,
                e.FirstName + " " + e.LastName,
                e.Department!.DepartmentName,
                e.PersonalInfo.Position,
                e.PersonalInfo.HiredDate))
            .ToListAsync();
}
