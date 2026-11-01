using System.Linq.Expressions;
using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class CourseCRUD(DataContext dataContext)
{
    private static readonly Dictionary<string, Expression<Func<Course, object?>>> SortColumns = new()
    {
        ["name"] = x => x.CourseName,
        ["department"] = x => x.Department!.DepartmentName,
        ["level"] = x => x.LevelId,
        ["duration"] = x => x.Duration,
        ["credit"] = x => x.Credit,
    };

    //Create Course
    public async Task SaveCourseAsync(Course course)
    {
        dataContext.Courses.Add(course);
        await dataContext.SaveChangesAsync();
    }

    //Read Course
    public Task<Course?> GetCourseByIdAsync(int courseId) =>
        dataContext.Courses
            .Include(x => x.Department)
            .Include(x => x.Level)
            .FirstOrDefaultAsync(x => x.CourseId == courseId);

    public Task<List<Course>> GetAllCoursesAsync() =>
        dataContext.Courses.AsNoTracking().Include(x => x.Level).Include(x => x.Department).OrderBy(x => x.LevelId).ThenBy(x => x.CourseName).ToListAsync();

    public Task<PagedResult<Course>> GetCoursesAsync(ListQuery query)
    {
        IQueryable<Course> courses = dataContext.Courses.AsNoTracking()
            .Include(x => x.Department)
            .Include(x => x.Level);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim().ToLower();
            courses = courses.Where(x => x.CourseName.ToLower().Contains(s)
                || (x.Description != null && x.Description.ToLower().Contains(s))
                || x.Department!.DepartmentName.ToLower().Contains(s));
        }

        return courses.ApplySort(query, SortColumns).ToPagedResultAsync(query);
    }

    //Update Course
    public async Task UpdateCourseAsync(Course course)
    {
        dataContext.Courses.Update(course);
        await dataContext.SaveChangesAsync();
    }

    //Delete Course
    public async Task RemoveCourseAsync(Course course)
    {
        dataContext.Courses.Remove(course);
        await dataContext.SaveChangesAsync();
    }
}
