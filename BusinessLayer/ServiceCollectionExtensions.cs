using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers the database context, data access classes and business services.</summary>
    public static IServiceCollection AddCollegeCms(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<DepartmentCRUD>();
        services.AddScoped<LevelCRUD>();
        services.AddScoped<CourseCRUD>();
        services.AddScoped<EmployeeCRUD>();
        services.AddScoped<StudentCRUD>();
        services.AddScoped<EnrollmentCRUD>();
        services.AddScoped<UserCRUD>();
        services.AddScoped<StatisticsCRUD>();

        services.AddScoped<DepartmentService>();
        services.AddScoped<LevelService>();
        services.AddScoped<CourseService>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<StudentService>();
        services.AddScoped<UserService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<DataSeeder>();

        return services;
    }
}
