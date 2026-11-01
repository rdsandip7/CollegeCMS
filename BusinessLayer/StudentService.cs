using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer;

public class StudentService(StudentCRUD studentTableActions, EnrollmentCRUD enrollmentTableActions, CourseCRUD courseTableActions)
{
    public Task<PagedResult<Student>> GetStudentsAsync(ListQuery query) => studentTableActions.GetStudentsAsync(query);

    public Task<Student?> GetStudentByIdAsync(string studentId) => studentTableActions.GetStudentByIdAsync(studentId);

    /// <summary>Saves a new student and returns the generated id (e.g. STU-2026-0007).</summary>
    public async Task<string> SaveStudentAsync(Student student)
    {
        student.StudentId = await GenerateStudentIdAsync(student.PersonalInfo.AdmissionDate.Year);
        student.PersonalInfo.StudentId = student.StudentId;
        student.InsertedDate = DateTime.Now;
        student.PersonalInfo.InsertedDate = DateTime.Now;
        await studentTableActions.SaveStudentAsync(student);
        return student.StudentId;
    }

    public async Task<OperationResult> UpdateStudentAsync(Student changes)
    {
        var student = await studentTableActions.GetStudentByIdAsync(changes.StudentId);
        if (student is null)
            return OperationResult.Fail("Student not found.");

        student.FirstName = changes.FirstName;
        student.MiddleName = changes.MiddleName;
        student.LastName = changes.LastName;
        student.DepartmentId = changes.DepartmentId;
        student.LevelId = changes.LevelId;
        student.UpdatedDate = DateTime.Now;

        var info = student.PersonalInfo;
        var newInfo = changes.PersonalInfo;
        info.Gender = newInfo.Gender;
        info.DateOfBirth = newInfo.DateOfBirth;
        info.CitizenshipNumber = newInfo.CitizenshipNumber;
        info.Email = newInfo.Email;
        info.Address = newInfo.Address;
        info.ContactNumber = newInfo.ContactNumber;
        info.GuardianName = newInfo.GuardianName;
        info.GuardianRelationship = newInfo.GuardianRelationship;
        info.GuardianContactNumber = newInfo.GuardianContactNumber;
        info.AdmissionDate = newInfo.AdmissionDate;
        info.UpdatedDate = DateTime.Now;

        await studentTableActions.UpdateStudentAsync(student);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteStudentAsync(string studentId)
    {
        var student = await studentTableActions.GetStudentByIdAsync(studentId);
        if (student is null)
            return OperationResult.Fail("Student not found.");

        await studentTableActions.RemoveStudentAsync(student);
        return OperationResult.Success;
    }

    public async Task<OperationResult> EnrollAsync(string studentId, int courseId)
    {
        if (await studentTableActions.GetStudentByIdAsync(studentId) is null)
            return OperationResult.Fail("Student not found.");
        if (await courseTableActions.GetCourseByIdAsync(courseId) is null)
            return OperationResult.Fail("Course not found.");
        if (await enrollmentTableActions.IsEnrolledAsync(studentId, courseId))
            return OperationResult.Fail("The student is already enrolled in this course.");

        await enrollmentTableActions.SaveEnrollmentAsync(new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledDate = DateTime.Today,
            InsertedDate = DateTime.Now,
        });
        return OperationResult.Success;
    }

    public async Task<OperationResult> UnenrollAsync(int enrollmentId)
    {
        var enrollment = await enrollmentTableActions.GetEnrollmentAsync(enrollmentId);
        if (enrollment is null)
            return OperationResult.Fail("Enrollment not found.");

        await enrollmentTableActions.RemoveEnrollmentAsync(enrollment);
        return OperationResult.Success;
    }

    private async Task<string> GenerateStudentIdAsync(int admissionYear)
    {
        var prefix = $"STU-{admissionYear}-";
        var last = await studentTableActions.GetLastStudentIdAsync(prefix);
        var next = last is not null && int.TryParse(last[prefix.Length..], out var n) ? n + 1 : 1;
        return $"{prefix}{next:D4}";
    }
}
