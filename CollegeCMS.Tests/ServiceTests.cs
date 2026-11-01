using CoreLayer;

namespace CollegeCMS.Tests;

public class ServiceTests
{
    [Fact]
    public async Task New_students_get_sequential_ids_for_their_admission_year()
    {
        await using var db = await TestDatabase.CreateAsync();
        var level = (await db.Levels.GetAllLevelsAsync())[0];
        var department = (await db.Departments.GetAllDepartmentsAsync())[0];

        // The seed data already holds STU-2026-0001 … 0006.
        var first = await db.Students.SaveStudentAsync(NewStudent(department.DepartmentId, level.LevelId, 2026));
        var second = await db.Students.SaveStudentAsync(NewStudent(department.DepartmentId, level.LevelId, 2026));
        var otherYear = await db.Students.SaveStudentAsync(NewStudent(department.DepartmentId, level.LevelId, 2027));

        Assert.Equal("STU-2026-0007", first);
        Assert.Equal("STU-2026-0008", second);
        Assert.Equal("STU-2027-0001", otherYear);
        Assert.NotNull((await db.Students.GetStudentByIdAsync(first))?.PersonalInfo);
    }

    [Fact]
    public async Task Department_with_students_cannot_be_deleted()
    {
        await using var db = await TestDatabase.CreateAsync();
        var department = (await db.Departments.GetAllDepartmentsAsync())[0];

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
    public async Task Course_list_supports_search_sort_and_paging()
    {
        await using var db = await TestDatabase.CreateAsync();

        var computer = await db.Courses.GetCoursesAsync(new ListQuery(Search: "computer", Sort: "name_desc"));
        Assert.Equal(3, computer.TotalCount);
        Assert.Equal("Programming in C", computer.Items[0].CourseName);
        Assert.All(computer.Items, c => Assert.Equal("Computer Engineering", c.Department!.DepartmentName));

        var page2 = await db.Courses.GetCoursesAsync(new ListQuery(Sort: "credit", Page: 2, PageSize: 3));
        Assert.Equal(2, page2.PageNumber);
        Assert.Equal(3, page2.PageCount);
        Assert.Equal(3, page2.Items.Count);
    }

    [Fact]
    public async Task Paging_past_the_end_returns_the_last_page()
    {
        await using var db = await TestDatabase.CreateAsync();

        var result = await db.Students.GetStudentsAsync(new ListQuery(Page: 99, PageSize: 4));

        Assert.Equal(2, result.PageNumber);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task Enrolling_twice_in_the_same_course_is_refused()
    {
        await using var db = await TestDatabase.CreateAsync();
        var course = (await db.Courses.GetAllCoursesAsync()).Single(c => c.CourseName == "Web Technology");

        Assert.True((await db.Students.EnrollAsync("STU-2026-0001", course.CourseId)).Succeeded);
        Assert.False((await db.Students.EnrollAsync("STU-2026-0001", course.CourseId)).Succeeded);
    }

    [Fact]
    public async Task Registered_users_can_sign_in_and_get_linked_to_their_employee_record()
    {
        await using var db = await TestDatabase.CreateAsync();

        var result = await db.Users.RegisterAsync("gita", "s3cret-pass", "GITA.RAI@mtech.edu.np");

        Assert.True(result.Succeeded);
        Assert.NotNull(await db.Users.AuthenticateAsync("gita", "s3cret-pass"));
        Assert.Null(await db.Users.AuthenticateAsync("gita", "wrong"));
        Assert.False((await db.Users.RegisterAsync("GITA", "another-pass", "x@example.com")).Succeeded);

        var employee = await db.Context.Employees.FindAsync(2);
        Assert.NotNull(employee?.UserId);
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
