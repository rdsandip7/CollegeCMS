using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class EnrollmentCRUD(DataContext dataContext)
{
    //Create Enrollment
    public async Task SaveEnrollmentAsync(Enrollment enrollment)
    {
        dataContext.Enrollments.Add(enrollment);
        await dataContext.SaveChangesAsync();
    }

    //Read Enrollment
    public Task<Enrollment?> GetEnrollmentAsync(int enrollmentId) =>
        dataContext.Enrollments.FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId);

    public Task<bool> IsEnrolledAsync(string studentId, int courseId) =>
        dataContext.Enrollments.AnyAsync(x => x.StudentId == studentId && x.CourseId == courseId);

    //Delete Enrollment
    public async Task RemoveEnrollmentAsync(Enrollment enrollment)
    {
        dataContext.Enrollments.Remove(enrollment);
        await dataContext.SaveChangesAsync();
    }
}
