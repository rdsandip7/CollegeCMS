using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeePersonalInfo> EmployeePersonalInfos => Set<EmployeePersonalInfo>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentPersonalInfo> StudentPersonalInfos => Set<StudentPersonalInfo>();
    public DbSet<Level> Levels => Set<Level>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateTableDepartment(modelBuilder);
        CreateTableEmployee(modelBuilder);
        CreateTableStudent(modelBuilder);
        CreateTableLevel(modelBuilder);
        CreateTableCourse(modelBuilder);
        CreateTableEnrollment(modelBuilder);
        CreateTableUser(modelBuilder);
    }

    private static void CreateTableDepartment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(e =>
        {
            e.ToTable("Departments");
            e.Property(x => x.DepartmentName).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.DepartmentName).IsUnique();
        });
    }

    private static void CreateTableEmployee(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(e =>
        {
            e.ToTable("Employees");
            e.Property(x => x.Email).HasMaxLength(200).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
            e.Ignore(x => x.FullName);

            // A department that still has staff cannot be deleted.
            e.HasOne(x => x.Department)
             .WithMany(d => d.Employees)
             .HasForeignKey(x => x.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            // Employee and EmployeePersonal share a primary key (one-to-one).
            e.HasOne(x => x.PersonalInfo)
             .WithOne(p => p.Employee)
             .HasForeignKey<EmployeePersonalInfo>(p => p.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EmployeePersonalInfo>(e =>
        {
            e.ToTable("EmployeePersonal");
            e.HasKey(x => x.EmployeeId);
            e.Property(x => x.EmployeeId).ValueGeneratedNever();
        });
    }

    private static void CreateTableStudent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(e =>
        {
            e.ToTable("Students");
            e.HasKey(x => x.StudentId);
            e.Property(x => x.StudentId).HasMaxLength(20).ValueGeneratedNever();
            e.Ignore(x => x.FullName);

            e.HasOne(x => x.Department)
             .WithMany(d => d.Students)
             .HasForeignKey(x => x.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Level)
             .WithMany(l => l.Students)
             .HasForeignKey(x => x.LevelId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.PersonalInfo)
             .WithOne(p => p.Student)
             .HasForeignKey<StudentPersonalInfo>(p => p.StudentId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StudentPersonalInfo>(e =>
        {
            e.ToTable("StudentPersonalInfo");
            e.HasKey(x => x.StudentId);
        });
    }

    private static void CreateTableLevel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Level>(e =>
        {
            e.ToTable("Levels");
            e.Ignore(x => x.DisplayName);
            e.Property(x => x.Semester).HasMaxLength(50).IsRequired();
        });
    }

    private static void CreateTableCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(e =>
        {
            e.ToTable("Courses");
            e.Property(x => x.CourseName).HasMaxLength(150).IsRequired();

            e.HasOne(x => x.Department)
             .WithMany(d => d.Courses)
             .HasForeignKey(x => x.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Level)
             .WithMany(l => l.Courses)
             .HasForeignKey(x => x.LevelId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void CreateTableEnrollment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>(e =>
        {
            e.ToTable("Enrollments");
            e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();

            e.HasOne(x => x.Student)
             .WithMany(s => s.Enrollments)
             .HasForeignKey(x => x.StudentId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Course)
             .WithMany(c => c.Enrollments)
             .HasForeignKey(x => x.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void CreateTableUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.Property(x => x.Username).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Username).IsUnique();
            e.Property(x => x.Role).HasMaxLength(20);
        });
    }
}
