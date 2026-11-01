using CoreLayer;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer;

/// <summary>Creates the database on first run and fills it with demo data so the app has something to show.</summary>
public class DataSeeder(DataContext dataContext)
{
    public async Task SeedAsync(string adminUsername, string adminPassword)
    {
        await dataContext.Database.EnsureCreatedAsync();

        if (!await dataContext.Users.AnyAsync())
        {
            dataContext.Users.Add(new User
            {
                Username = adminUsername,
                PasswordHash = PasswordHasher.Hash(adminPassword),
                Role = Roles.Admin,
                InsertedDate = DateTime.Now,
            });
            await dataContext.SaveChangesAsync();
        }

        if (await dataContext.Departments.AnyAsync())
            return;

        var now = DateTime.Now;
        var civil = new Department { DepartmentName = "Civil Engineering", DepartmentHead = "Dr. Ramesh Shrestha", Description = "Structures, surveying, transportation and water resources.", EstablishedDate = new DateTime(2000, 7, 15), InsertedDate = now };
        var computer = new Department { DepartmentName = "Computer Engineering", DepartmentHead = "Dr. Sita Karki", Description = "Software, hardware, networks and embedded systems.", EstablishedDate = new DateTime(2001, 3, 1), InsertedDate = now };
        var electrical = new Department { DepartmentName = "Electrical Engineering", DepartmentHead = "Er. Bikash Thapa", Description = "Power systems, machines and electronics.", EstablishedDate = new DateTime(2003, 8, 20), InsertedDate = now };
        var it = new Department { DepartmentName = "BSc. IT", DepartmentHead = "Mr. Anil Maharjan", Description = "Information technology for students from any intermediate stream.", EstablishedDate = new DateTime(2008, 1, 10), InsertedDate = now };
        dataContext.Departments.AddRange(civil, computer, electrical, it);

        var levels = new[]
        {
            new Level { Year = 2026, Semester = "First Semester", Description = "Foundation courses", InsertedDate = now },
            new Level { Year = 2026, Semester = "Second Semester", Description = "Core fundamentals", InsertedDate = now },
            new Level { Year = 2027, Semester = "Third Semester", Description = "Intermediate subjects", InsertedDate = now },
            new Level { Year = 2027, Semester = "Fourth Semester", Description = "Specialisation begins", InsertedDate = now },
        };
        dataContext.Levels.AddRange(levels);

        var courses = new[]
        {
            new Course { CourseName = "Engineering Mathematics I", Department = civil, Level = levels[0], Credit = 3, Duration = "16 weeks", Description = "Calculus, matrices and differential equations.", InsertedDate = now },
            new Course { CourseName = "Structural Analysis", Department = civil, Level = levels[2], Credit = 4, Duration = "16 weeks", Description = "Statically determinate and indeterminate structures.", InsertedDate = now },
            new Course { CourseName = "Programming in C", Department = computer, Level = levels[0], Credit = 3, Duration = "16 weeks", Description = "Procedural programming fundamentals.", InsertedDate = now },
            new Course { CourseName = "Data Structures and Algorithms", Department = computer, Level = levels[2], Credit = 4, Duration = "16 weeks", Description = "Lists, trees, graphs, sorting and complexity.", InsertedDate = now },
            new Course { CourseName = "Database Management Systems", Department = computer, Level = levels[3], Credit = 3, Duration = "16 weeks", Description = "Relational modelling, SQL and transactions.", InsertedDate = now },
            new Course { CourseName = "Circuit Theory", Department = electrical, Level = levels[1], Credit = 3, Duration = "16 weeks", Description = "Network theorems and AC/DC analysis.", InsertedDate = now },
            new Course { CourseName = "Web Technology", Department = it, Level = levels[1], Credit = 3, Duration = "16 weeks", Description = "HTML, CSS, JavaScript and server-side basics.", InsertedDate = now },
            new Course { CourseName = "Object-Oriented Programming", Department = it, Level = levels[2], Credit = 3, Duration = "16 weeks", Description = "Classes, inheritance and design with C#.", InsertedDate = now },
        };
        dataContext.Courses.AddRange(courses);

        dataContext.Employees.AddRange(
            NewEmployee("Hari", null, "Adhikari", "hari.adhikari@mtech.edu.np", civil, "Lecturer", "Male", new DateTime(2015, 4, 1)),
            NewEmployee("Gita", "Kumari", "Rai", "gita.rai@mtech.edu.np", computer, "Associate Professor", "Female", new DateTime(2012, 9, 15)),
            NewEmployee("Suresh", null, "Gurung", "suresh.gurung@mtech.edu.np", electrical, "Lab Instructor", "Male", new DateTime(2019, 2, 10)),
            NewEmployee("Anita", null, "Shakya", "anita.shakya@mtech.edu.np", it, "Lecturer", "Female", new DateTime(2020, 6, 1)),
            NewEmployee("Prakash", "Raj", "Joshi", "prakash.joshi@mtech.edu.np", computer, "Accountant", "Male", new DateTime(2010, 1, 5)));

        var students = new[]
        {
            NewStudent("STU-2026-0001", "Aarav", null, "Sharma", computer, levels[0], "Male", new DateTime(2006, 5, 12)),
            NewStudent("STU-2026-0002", "Priya", null, "Tamang", it, levels[1], "Female", new DateTime(2005, 11, 3)),
            NewStudent("STU-2026-0003", "Rohan", "Bahadur", "KC", civil, levels[0], "Male", new DateTime(2006, 2, 21)),
            NewStudent("STU-2026-0004", "Sneha", null, "Basnet", electrical, levels[1], "Female", new DateTime(2005, 8, 30)),
            NewStudent("STU-2026-0005", "Nabin", null, "Lama", computer, levels[2], "Male", new DateTime(2004, 12, 9)),
            NewStudent("STU-2026-0006", "Asmita", null, "Poudel", it, levels[2], "Female", new DateTime(2004, 7, 17)),
        };
        dataContext.Students.AddRange(students);

        dataContext.Enrollments.AddRange(
            NewEnrollment(students[0], courses[2]),
            NewEnrollment(students[1], courses[6]),
            NewEnrollment(students[2], courses[0]),
            NewEnrollment(students[3], courses[5]),
            NewEnrollment(students[4], courses[3]),
            NewEnrollment(students[4], courses[4]),
            NewEnrollment(students[5], courses[7]));

        await dataContext.SaveChangesAsync();

        Employee NewEmployee(string first, string? middle, string last, string email, Department dept, string position, string gender, DateTime hired) => new()
        {
            FirstName = first, MiddleName = middle, LastName = last, Email = email, Department = dept, InsertedDate = now,
            PersonalInfo = new EmployeePersonalInfo { Position = position, Gender = gender, HiredDate = hired, Address = "Kathmandu", Phone = "98" + Random.Shared.Next(10000000, 99999999), InsertedDate = now },
        };

        Student NewStudent(string id, string first, string? middle, string last, Department dept, Level level, string gender, DateTime dob) => new()
        {
            StudentId = id, FirstName = first, MiddleName = middle, LastName = last, Department = dept, Level = level, InsertedDate = now,
            PersonalInfo = new StudentPersonalInfo
            {
                StudentId = id, Gender = gender, DateOfBirth = dob, AdmissionDate = new DateTime(2026, 2, 1),
                Email = $"{first.ToLower()}.{last.ToLower()}@student.mtech.edu.np", Address = "Lalitpur",
                ContactNumber = "98" + Random.Shared.Next(10000000, 99999999), GuardianName = $"Mr. {last}", GuardianRelationship = "Father",
                GuardianContactNumber = "98" + Random.Shared.Next(10000000, 99999999), InsertedDate = now,
            },
        };

        Enrollment NewEnrollment(Student student, Course course) => new()
        {
            Student = student, Course = course, EnrolledDate = new DateTime(2026, 2, 15), InsertedDate = now,
        };
    }
}
