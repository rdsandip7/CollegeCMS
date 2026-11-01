using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace CollegeCMS.Tests;

public class ServiceTests
{
    [Fact]
    public async Task Seeded_college_has_realistic_size_and_shape()
    {
        await using var db = await TestDatabase.CreateAsync();
        var context = db.Context;

        Assert.InRange(await context.Students.CountAsync(), 550, 750);
        Assert.InRange(await context.Employees.CountAsync(), 70, 110);
        Assert.Equal(160, await context.Courses.CountAsync());
        Assert.Equal(8, await context.Levels.CountAsync());
        Assert.Equal(4, await context.Departments.CountAsync(d => d.Students.Any()));

        // Every student is enrolled in their current semester's courses, and every department has a head.
        Assert.False(await context.Students.AnyAsync(s => !s.Enrollments.Any()));
        Assert.False(await context.Departments.AnyAsync(d => d.DepartmentHead == null));
        Assert.False(await context.Enrollments.AnyAsync(e => e.Course!.LevelId != e.Student!.LevelId));
    }

    [Fact]
    public async Task Seed_data_is_the_same_every_time()
    {
        await using var first = await TestDatabase.CreateAsync();
        await using var second = await TestDatabase.CreateAsync();

        var a = await first.Context.Students.OrderBy(s => s.StudentId).Select(s => s.StudentId + s.FirstName + s.LastName).Take(50).ToListAsync();
        var b = await second.Context.Students.OrderBy(s => s.StudentId).Select(s => s.StudentId + s.FirstName + s.LastName).Take(50).ToListAsync();

        Assert.Equal(a, b);
    }

    [Fact]
    public async Task New_students_get_the_next_id_for_their_admission_year()
    {
        await using var db = await TestDatabase.CreateAsync();
        var student = await db.Context.Students.Include(s => s.PersonalInfo).OrderByDescending(s => s.StudentId).FirstAsync();
        var intake = student.PersonalInfo.AdmissionDate.Year;
        var lastNumber = int.Parse(student.StudentId[^4..]);

        var next = await db.Students.SaveStudentAsync(NewStudent(student.DepartmentId, student.LevelId, intake));
        var future1 = await db.Students.SaveStudentAsync(NewStudent(student.DepartmentId, student.LevelId, 2099));
        var future2 = await db.Students.SaveStudentAsync(NewStudent(student.DepartmentId, student.LevelId, 2099));

        Assert.Equal($"STU-{intake}-{lastNumber + 1:D4}", next);
        Assert.Equal("STU-2099-0001", future1);
        Assert.Equal("STU-2099-0002", future2);
        Assert.NotNull((await db.Students.GetStudentByIdAsync(next))?.PersonalInfo);
    }

    [Fact]
    public async Task Department_with_students_cannot_be_deleted()
    {
        await using var db = await TestDatabase.CreateAsync();
        var department = await db.Context.Departments.FirstAsync(d => d.DepartmentName == "Computer Engineering");

        var result = await db.Departments.DeleteDepartmentAsync(department.DepartmentId);

        Assert.False(result.Succeeded);
        Assert.Contains("still has", result.Error);
    }

    [Fact]
    public async Task Empty_department_can_be_deleted()
    {
        await using var db = await TestDatabase.CreateAsync();
        await db.Departments.SaveDepartmentAsync(new Department { DepartmentName = "Architecture" });
        var created = (await db.Departments.GetAllDepartmentsAsync()).Single(d => d.DepartmentName == "Architecture");

        var result = await db.Departments.DeleteDepartmentAsync(created.DepartmentId);

        Assert.True(result.Succeeded);
        Assert.Null(await db.Departments.GetDepartmentByIdAsync(created.DepartmentId));
    }

    [Fact]
    public async Task Duplicate_department_names_are_rejected_case_insensitively()
    {
        await using var db = await TestDatabase.CreateAsync();

        var result = await db.Departments.SaveDepartmentAsync(new Department { DepartmentName = "computer engineering" });

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task Course_list_supports_search_and_sort()
    {
        await using var db = await TestDatabase.CreateAsync();

        var result = await db.Courses.GetCoursesAsync(new ListQuery(Search: "structures", Sort: "name_desc", PageSize: 50));

        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, c => Assert.True(
            c.CourseName.Contains("Structures", StringComparison.OrdinalIgnoreCase)
            || (c.Description ?? "").Contains("structures", StringComparison.OrdinalIgnoreCase)
            || c.Department!.DepartmentName.Contains("structures", StringComparison.OrdinalIgnoreCase)));
        Assert.Equal(result.Items.Select(c => c.CourseName).OrderDescending(StringComparer.Ordinal), result.Items.Select(c => c.CourseName));
    }

    [Fact]
    public async Task Paging_returns_full_pages_and_clamps_past_the_end()
    {
        await using var db = await TestDatabase.CreateAsync();

        var page2 = await db.Students.GetStudentsAsync(new ListQuery(Page: 2, PageSize: 15));
        var beyond = await db.Students.GetStudentsAsync(new ListQuery(Page: 999, PageSize: 15));

        Assert.Equal(15, page2.Items.Count);
        Assert.Equal((int)Math.Ceiling(page2.TotalCount / 15.0), page2.PageCount);
        Assert.Equal(page2.PageCount, beyond.PageNumber);
        Assert.InRange(beyond.Items.Count, 1, 15);
    }

    [Fact]
    public async Task Enrolling_twice_in_the_same_course_is_refused()
    {
        await using var db = await TestDatabase.CreateAsync();
        var enrollment = await db.Context.Enrollments.FirstAsync();
        var otherCourse = await db.Context.Courses.FirstAsync(c => !c.Enrollments.Any(e => e.StudentId == enrollment.StudentId));

        Assert.False((await db.Students.EnrollAsync(enrollment.StudentId, enrollment.CourseId)).Succeeded);
        Assert.True((await db.Students.EnrollAsync(enrollment.StudentId, otherCourse.CourseId)).Succeeded);
        Assert.False((await db.Students.EnrollAsync(enrollment.StudentId, otherCourse.CourseId)).Succeeded);
    }

    [Fact]
    public async Task Registered_users_can_sign_in_and_get_linked_to_their_employee_record()
    {
        await using var db = await TestDatabase.CreateAsync();
        var employee = await db.Context.Employees.OrderBy(e => e.EmployeeId).FirstAsync();

        var result = await db.Users.RegisterAsync("staff.member", "s3cret-pass", employee.Email.ToUpperInvariant());

        Assert.True(result.Succeeded);
        Assert.NotNull(await db.Users.AuthenticateAsync("staff.member", "s3cret-pass"));
        Assert.Null(await db.Users.AuthenticateAsync("staff.member", "wrong"));
        Assert.False((await db.Users.RegisterAsync("STAFF.MEMBER", "another-pass", "x@example.com")).Succeeded);

        await db.Context.Entry(employee).ReloadAsync();
        Assert.NotNull(employee.UserId);
    }

    [Fact]
    public async Task Seeded_admin_password_is_stored_hashed()
    {
        await using var db = await TestDatabase.CreateAsync();

        var admin = db.Context.Users.Single(u => u.Username == "admin");

        Assert.Equal(Roles.Admin, admin.Role);
        Assert.DoesNotContain("Admin@123", admin.PasswordHash);
        Assert.NotNull(await db.Users.AuthenticateAsync("admin", "Admin@123"));
    }

    private static Student NewStudent(int departmentId, int levelId, int admissionYear) => new()
    {
        FirstName = "Test",
        LastName = "Student",
        DepartmentId = departmentId,
        LevelId = levelId,
        PersonalInfo = new StudentPersonalInfo
        {
            DateOfBirth = new DateTime(2005, 1, 1),
            AdmissionDate = new DateTime(admissionYear, 2, 1),
        },
    };
}
