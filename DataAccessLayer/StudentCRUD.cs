using System.Linq.Expressions;
using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class StudentCRUD(DataContext dataContext)
{
    private static readonly Dictionary<string, Expression<Func<Student, object?>>> SortColumns = new()
    {
        ["id"] = x => x.StudentId,
        ["name"] = x => x.FirstName,
        ["department"] = x => x.Department!.DepartmentName,
        ["level"] = x => x.Level!.Year,
        ["admission"] = x => x.PersonalInfo.AdmissionDate,
    };

    //Create Student (the personal info row is saved with it)
    public async Task SaveStudentAsync(Student student)
    {
        dataContext.Students.Add(student);
        await dataContext.SaveChangesAsync();
    }

    //Read Student
    public Task<Student?> GetStudentByIdAsync(string studentId) =>
        dataContext.Students
            .Include(x => x.Department)
            .Include(x => x.Level)
            .Include(x => x.PersonalInfo)
            .Include(x => x.Enrollments).ThenInclude(e => e.Course)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StudentId == studentId);

    /// <summary>Highest existing student id that starts with <paramref name="prefix"/>, e.g. "STU-2026-".</summary>
    public Task<string?> GetLastStudentIdAsync(string prefix) =>
        dataContext.Students
            .Where(x => x.StudentId.StartsWith(prefix))
            .OrderByDescending(x => x.StudentId)
            .Select(x => x.StudentId)
            .FirstOrDefaultAsync();

    public Task<PagedResult<Student>> GetStudentsAsync(ListQuery query)
    {
        IQueryable<Student> students = dataContext.Students.AsNoTracking()
            .Include(x => x.Department)
            .Include(x => x.Level)
            .Include(x => x.PersonalInfo);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim().ToLower();
            students = students.Where(x => x.StudentId.ToLower().Contains(s)
                || x.FirstName.ToLower().Contains(s)
                || (x.MiddleName != null && x.MiddleName.ToLower().Contains(s))
                || x.LastName.ToLower().Contains(s)
                || x.Department!.DepartmentName.ToLower().Contains(s)
                || (x.PersonalInfo.Email != null && x.PersonalInfo.Email.ToLower().Contains(s)));
        }

        return students.ApplySort(query, SortColumns).ToPagedResultAsync(query);
    }

    //Update Student
    public async Task UpdateStudentAsync(Student student)
    {
        dataContext.Students.Update(student);
        await dataContext.SaveChangesAsync();
    }

    //Delete Student (personal info and enrollments are removed by cascade)
    public async Task RemoveStudentAsync(Student student)
    {
        dataContext.Students.Remove(student);
        await dataContext.SaveChangesAsync();
    }
}
