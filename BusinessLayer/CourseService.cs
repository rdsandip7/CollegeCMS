using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer;

public class CourseService(CourseCRUD courseTableActions)
{
    public Task<PagedResult<Course>> GetCoursesAsync(ListQuery query) => courseTableActions.GetCoursesAsync(query);

    public Task<List<Course>> GetAllCoursesAsync() => courseTableActions.GetAllCoursesAsync();

    public Task<Course?> GetCourseByIdAsync(int courseId) => courseTableActions.GetCourseByIdAsync(courseId);

    public async Task SaveCourseAsync(Course course)
    {
        course.InsertedDate = DateTime.Now;
        await courseTableActions.SaveCourseAsync(course);
    }

    public async Task<OperationResult> UpdateCourseAsync(Course changes)
    {
        var course = await courseTableActions.GetCourseByIdAsync(changes.CourseId);
        if (course is null)
            return OperationResult.Fail("Course not found.");

        course.CourseName = changes.CourseName;
        course.DepartmentId = changes.DepartmentId;
        course.LevelId = changes.LevelId;
        course.Description = changes.Description;
        course.Credit = changes.Credit;
        course.Duration = changes.Duration;
        course.UpdatedDate = DateTime.Now;
        await courseTableActions.UpdateCourseAsync(course);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteCourseAsync(int courseId)
    {
        var course = await courseTableActions.GetCourseByIdAsync(courseId);
        if (course is null)
            return OperationResult.Fail("Course not found.");

        await courseTableActions.RemoveCourseAsync(course);
        return OperationResult.Success;
    }
}
