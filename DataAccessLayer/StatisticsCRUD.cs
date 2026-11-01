using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public record CollegeStatistics(int Departments, int Levels, int Courses, int Students, int Employees, int Enrollments);

public record DepartmentHeadcount(string Department, int Students, int Employees, int Courses);

public class StatisticsCRUD(DataContext dataContext)
{
    public async Task<CollegeStatistics> GetStatisticsAsync() => new(
        await dataContext.Departments.CountAsync(),
        await dataContext.Levels.CountAsync(),
        await dataContext.Courses.CountAsync(),
        await dataContext.Students.CountAsync(),
        await dataContext.Employees.CountAsync(),
        await dataContext.Enrollments.CountAsync());

    public Task<List<DepartmentHeadcount>> GetDepartmentHeadcountsAsync() =>
        dataContext.Departments
            .OrderBy(d => d.DepartmentName)
            .Select(d => new DepartmentHeadcount(d.DepartmentName, d.Students.Count, d.Employees.Count, d.Courses.Count))
            .ToListAsync();
}
