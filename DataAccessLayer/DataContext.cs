using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CoreLayer;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    public class DataContext : DbContext
    {
        public DataContext() : base("CollegeCMS") { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePersonalInfo> EmployeePersonalInfos { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentPersonalInfo> StudentPersonalInfos { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            CreateTableDepartment(modelBuilder);
            CreateTableEmployee(modelBuilder);
            CreateTableEmployeePersonal(modelBuilder);
            CreateTableStudent(modelBuilder);
            CreateTableStudentPersonal(modelBuilder);
            CreateTableLevel(modelBuilder);
            CreateTableCourse(modelBuilder);
            CreateTableEnrollment(modelBuilder);
            CreateTableUser(modelBuilder);
        }

        private static void CreateTableDepartment(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Department>().HasKey(x => x.DepartmentId);
            modelBuilder.Entity<Department>().Property(x => x.DepartmentId).HasColumnName("DepartmentId");
            modelBuilder.Entity<Department>().Property(x => x.DepartmentName).HasColumnName("DepartmentName");
            modelBuilder.Entity<Department>().Property(x => x.Description).HasColumnName("Description");
            modelBuilder.Entity<Department>().Property(x => x.EstablishedDate).HasColumnName("EstablishedDate");
            modelBuilder.Entity<Department>().Property(x => x.DepartmentHead).HasColumnName("DepartmentHead");
            modelBuilder.Entity<Department>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<Department>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        }

        private static void CreateTableEmployee(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Employee>().HasKey(x => x.EmployeeId);
            modelBuilder.Entity<Employee>().Property(x => x.EmployeeId).HasColumnName("EmployeeId");
            modelBuilder.Entity<Employee>().Property(x => x.UserId).HasColumnName("UserId");
            modelBuilder.Entity<Employee>().Property(x => x.DepartmentId).HasColumnName("DepartmentId");
            modelBuilder.Entity<Employee>().Property(x => x.Email).HasColumnName("Email");
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).HasColumnName("FirstName");
            modelBuilder.Entity<Employee>().Property(x => x.MiddleName).HasColumnName("MiddleName");
            modelBuilder.Entity<Employee>().Property(x => x.LastName).HasColumnName("LastName");
            modelBuilder.Entity<Employee>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<Employee>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

            modelBuilder.Entity<Employee>()
             .HasRequired<Department>(s => s.Department)
             .WithMany(s => s.Employees)
             .HasForeignKey(s => s.DepartmentId);
        }
        private static void CreateTableEmployeePersonal(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeePersonalInfo>().ToTable("EmployeePersonal");
            modelBuilder.Entity<EmployeePersonalInfo>().HasKey(x => x.EmployeeId);
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.EmployeeId).HasColumnName("EmployeeId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.Phone).HasColumnName("Phone");
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.Address).HasColumnName("Address");
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.Position).HasColumnName("Position");            
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.Gender).HasColumnName("Gender");
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.HiredDate).HasColumnName("HiredDate");
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.ResignationDate).HasColumnName("ResignationDate");
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<EmployeePersonalInfo>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

            modelBuilder.Entity<Employee>().HasRequired(x => x.EmployeePersonalInfo).WithRequiredPrincipal(z => z.Employee);
        }

        private static void CreateTableStudent(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Student>().HasKey(x => x.StudentId);
            modelBuilder.Entity<Student>().Property(x => x.StudentId).HasColumnName("StudentId");
            modelBuilder.Entity<Student>().Property(x => x.DepartmentId).HasColumnName("DepartmentId");
            modelBuilder.Entity<Student>().Property(x => x.LevelId).HasColumnName("LevelId");
            modelBuilder.Entity<Student>().Property(x => x.FirstName).HasColumnName("FirstName");
            modelBuilder.Entity<Student>().Property(x => x.MiddleName).HasColumnName("MiddleName");
            modelBuilder.Entity<Student>().Property(x => x.LastName).HasColumnName("LastName");
            modelBuilder.Entity<Student>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<Student>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

            modelBuilder.Entity<Student>()
              .HasRequired<Department>(s => s.Department)
              .WithMany(s => s.Students)
              .HasForeignKey(s => s.DepartmentId);

            modelBuilder.Entity<Student>()
             .HasRequired<Level>(s => s.Level)
             .WithMany(s => s.Students)
             .HasForeignKey(s => s.LevelId);
        }
        private static void CreateTableStudentPersonal(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentPersonalInfo>().ToTable("StudentPersonalInfo");
            modelBuilder.Entity<StudentPersonalInfo>().HasKey(x => x.StudentId);
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.StudentId).HasColumnName("StudentId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.ContactNumber).HasColumnName("ContactNumber");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.Address).HasColumnName("Address");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.GuardianName).HasColumnName("GuardianName");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.GuardianContactNumber).HasColumnName("GuardianContactNumber");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.Gender).HasColumnName("Gender");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.Email).HasColumnName("Email");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.CitizenshipNumber).HasColumnName("CitizenshipNumber");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.GuardianRelationship).HasColumnName("GuardianRelationship");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.DateOfBirth).HasColumnName("DateOfBirth");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.AdmissionDate).HasColumnName("AdmissionDate");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<StudentPersonalInfo>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

            modelBuilder.Entity<Student>().HasRequired(x => x.StudentPersonalInfo).WithRequiredPrincipal(z => z.Student);
        }

        private static void CreateTableLevel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Level>().ToTable("Levels");
            modelBuilder.Entity<Level>().HasKey(x => x.LevelId);
            modelBuilder.Entity<Level>().Property(x => x.LevelId).HasColumnName("LevelId");
            modelBuilder.Entity<Level>().Property(x => x.Year).HasColumnName("Year");
            modelBuilder.Entity<Level>().Property(x => x.Semester).HasColumnName("Semester");
            modelBuilder.Entity<Level>().Property(x => x.Description).HasColumnName("Description");
            modelBuilder.Entity<Level>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<Level>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

        }
        private static void CreateTableCourse(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Course>().HasKey(x => x.CourseId);
            modelBuilder.Entity<Course>().Property(x => x.CourseId).HasColumnName("CourseId");
            modelBuilder.Entity<Course>().Property(x => x.DepartmentId).HasColumnName("DepartmentId");
            modelBuilder.Entity<Course>().Property(x => x.LevelId).HasColumnName("LevelId");
            modelBuilder.Entity<Course>().Property(x => x.CourseName).HasColumnName("CourseName");
            modelBuilder.Entity<Course>().Property(x => x.Description).HasColumnName("Description");
            modelBuilder.Entity<Course>().Property(x => x.Duration).HasColumnName("Duration");
            modelBuilder.Entity<Course>().Property(x => x.Credit).HasColumnName("Credit");
            modelBuilder.Entity<Course>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<Course>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");

            modelBuilder.Entity<Course>()
                .HasRequired<Department>(x => x.Departments)
                .WithMany(y => y.Courses)
                .HasForeignKey(x => x.DepartmentId);

            modelBuilder.Entity<Course>()
                .HasRequired<Level>(x => x.Levels)
                .WithMany(y => y.Courses)
                .HasForeignKey(x => x.LevelId);
        }
        private static void CreateTableEnrollment(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>().ToTable("Enrollments");
            modelBuilder.Entity<Enrollment>().HasKey(x => x.EnrollmentId);
            modelBuilder.Entity<Enrollment>().Property(x => x.EnrollmentId).HasColumnName("EnrollmentId");
            modelBuilder.Entity<Enrollment>().Property(x => x.StudentId).HasColumnName("StudentId");
            modelBuilder.Entity<Enrollment>().Property(x => x.CourseId).HasColumnName("CourseId");
            modelBuilder.Entity<Enrollment>().Property(x => x.EnrolledDate).HasColumnName("EnrolledDate");
            modelBuilder.Entity<Enrollment>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<Enrollment>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        }

        private static void CreateTableUser(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(x => x.UserId);
            modelBuilder.Entity<User>().Property(x => x.UserId).HasColumnName("UserId");
            modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("Username");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("Password");
            //modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("Email");
            modelBuilder.Entity<User>().Property(x => x.Role).HasColumnName("Role");
            modelBuilder.Entity<User>().Property(x => x.InsertedDate).HasColumnName("InsertedDate");
            modelBuilder.Entity<User>().Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        }
    }
}
