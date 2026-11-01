using BusinessLayer;
using DataAccessLayer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CollegeCMS.Tests;

/// <summary>A fresh, seeded in-memory SQLite database per test, plus ready-made services.</summary>
public sealed class TestDatabase : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    public DataContext Context { get; }

    private TestDatabase(SqliteConnection connection, DataContext context)
    {
        _connection = connection;
        Context = context;
    }

    public static async Task<TestDatabase> CreateAsync(bool seed = true)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlite(connection).Options);

        if (seed)
            await new DataSeeder(context).SeedAsync("admin", "Admin@123");
        else
            await context.Database.EnsureCreatedAsync();

        return new TestDatabase(connection, context);
    }

    public DepartmentService Departments => new(new DepartmentCRUD(Context));
    public LevelService Levels => new(new LevelCRUD(Context));
    public CourseService Courses => new(new CourseCRUD(Context));
    public EmployeeService Employees => new(new EmployeeCRUD(Context));
    public StudentService Students => new(new StudentCRUD(Context), new EnrollmentCRUD(Context), new CourseCRUD(Context));
    public UserService Users => new(new UserCRUD(Context), new EmployeeCRUD(Context));

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
