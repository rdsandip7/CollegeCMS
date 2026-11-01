using CoreLayer;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using static BusinessLayer.SeedData;

namespace BusinessLayer;

/// <summary>
/// Creates the database on first run and fills it with a realistic college: four programmes,
/// eight semesters of curriculum, four intake batches of students and a full staff list.
/// Generation uses a fixed random seed, so the same data comes out every time.
/// </summary>
public class DataSeeder(DataContext dataContext)
{
    private readonly Random _random = new(2026);
    private readonly HashSet<string> _usedEmails = new(StringComparer.OrdinalIgnoreCase);

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

        // Faster bulk insert: one change scan at the end instead of one per Add.
        dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
        try
        {
            SeedCollege();
            dataContext.ChangeTracker.DetectChanges();
            await dataContext.SaveChangesAsync();
        }
        finally
        {
            dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    private void SeedCollege()
    {
        var today = DateTime.Today;

        // Semesters (levels). Year = programme year 1-4.
        var levels = SemesterNames.Select((name, i) => new Level
        {
            Year = i / 2 + 1,
            Semester = name,
            Description = i % 2 == 0 ? "Odd semester · February to July" : "Even semester · August to January",
            InsertedDate = today,
        }).ToArray();
        dataContext.Levels.AddRange(levels);

        // Departments: four academic programmes plus two that only employ staff.
        var programmeDepartments = Programmes.ToDictionary(p => p, p => new Department
        {
            DepartmentName = p.Name,
            Description = p.Description,
            EstablishedDate = p.Established,
            InsertedDate = today,
        });
        var science = new Department
        {
            DepartmentName = "Science & Humanities",
            Description = "Mathematics, physics, chemistry, drawing and communication courses for every engineering programme.",
            EstablishedDate = new DateTime(2000, 7, 16),
            InsertedDate = today,
        };
        var administration = new Department
        {
            DepartmentName = "Administration",
            Description = "Principal's office, registry, examinations, finance, library and IT services.",
            EstablishedDate = new DateTime(2000, 7, 16),
            InsertedDate = today,
        };
        dataContext.Departments.AddRange(programmeDepartments.Values);
        dataContext.Departments.AddRange(science, administration);

        // Courses. First-year engineering courses belong to Science & Humanities and are shared.
        var courses = new Dictionary<(Department, int), List<Course>>();
        for (var semester = 0; semester < 2; semester++)
            courses[(science, semester)] = AddCourses(science, levels[semester], CommonEngineeringYear[semester]);
        foreach (var programme in Programmes)
        {
            var department = programmeDepartments[programme];
            for (var semester = 0; semester < 8; semester++)
            {
                if (programme.Curriculum[semester].Length > 0)
                    courses[(department, semester)] = AddCourses(department, levels[semester], programme.Curriculum[semester]);
            }
        }

        // Staff.
        foreach (var department in programmeDepartments.Values)
        {
            var staff = AddStaff(department, DepartmentStaffing, today);
            department.DepartmentHead = "Dr. " + staff[0].FullName;
        }
        science.DepartmentHead = "Dr. " + AddStaff(science, ScienceStaffing, today)[0].FullName;
        administration.DepartmentHead = "Prof. Dr. " + AddStaff(administration, AdministrationStaffing, today)[0].FullName;

        // Students: four February intakes, the oldest close to graduating.
        var currentIntake = today.Month >= 2 ? today.Year : today.Year - 1;
        var evenSemester = today.Month is >= 8 or 1;
        var semesterStart = evenSemester ? new DateTime(currentIntake, 8, 15) : new DateTime(currentIntake, 2, 15);
        foreach (var programme in Programmes)
        {
            var department = programmeDepartments[programme];
            for (var yearsIn = 3; yearsIn >= 0; yearsIn--)
            {
                var intake = currentIntake - yearsIn;
                var semester = yearsIn * 2 + (evenSemester ? 1 : 0);
                var courseOwner = semester < 2 && programme != InformationTechnology ? science : department;
                var currentCourses = courses[(courseOwner, semester)];

                // Each year a few students drop out or transfer.
                var retention = 1 - yearsIn * 0.035 - _random.NextDouble() * 0.03;
                var batchSize = (int)Math.Round(programme.IntakeSeats * retention);
                for (var seat = 1; seat <= batchSize; seat++)
                    AddStudent(programme, department, levels[semester], intake, semesterStart, currentCourses);
            }
        }
    }

    private List<Course> AddCourses(Department department, Level level, string[] names)
    {
        var list = names.Select(name =>
        {
            var (credit, kind) = name switch
            {
                _ when name.Contains("Project") || name == "Internship" => (name.StartsWith("Major") || name == "Internship" ? 4 : 2, "Supervised project work with milestone reviews and a final defence."),
                _ when name.Contains("Drawing") || name.Contains("Workshop") || name.Contains("Camp") => (2, "Practical course with studio or field sessions every week."),
                _ when name.Contains("Mathematics") || name.Contains("Physics") || name.Contains("Chemistry") || name.Contains("Theory of Structures") => (4, "Core theory course: four lectures and one tutorial per week."),
                _ when name.StartsWith("Elective") => (3, "Elective chosen by the department board for this batch."),
                _ => (3, "Core course: three lectures and a two-hour lab or tutorial per week."),
            };
            return new Course
            {
                CourseName = name,
                Department = department,
                Level = level,
                Credit = credit,
                Duration = name.Contains("Camp") ? "2 weeks (field)" : name == "Internship" ? "10 weeks" : "15 weeks",
                Description = kind,
                InsertedDate = DateTime.Today,
            };
        }).ToList();
        dataContext.Courses.AddRange(list);
        return list;
    }

    private List<Employee> AddStaff(Department department, (string Position, int Count)[] staffing, DateTime today)
    {
        var staff = new List<Employee>();
        var seniority = 0;
        foreach (var (position, count) in staffing)
        {
            for (var i = 0; i < count; i++, seniority++)
            {
                var female = _random.NextDouble() < 0.32;
                var (first, middle, last) = RandomName(female);

                // Senior posts were hired earlier; juniors joined recently.
                var earliest = Math.Max(department.EstablishedDate!.Value.Year, 2001 + seniority);
                var hired = RandomDate(new DateTime(Math.Min(earliest, today.Year - 1), 1, 1), today.AddMonths(-2));
                DateTime? resigned = null;
                if (seniority > 2 && _random.NextDouble() < 0.07)
                    resigned = RandomDate(hired.AddMonths(8), today.AddDays(-14));

                var employee = new Employee
                {
                    FirstName = first,
                    MiddleName = middle,
                    LastName = last,
                    Email = UniqueEmail(first, last, "mtech.edu.np"),
                    Department = department,
                    InsertedDate = hired,
                    PersonalInfo = new EmployeePersonalInfo
                    {
                        Gender = female ? "Female" : "Male",
                        Position = position,
                        Address = Pick(Localities[..14]),
                        Phone = MobileNumber(),
                        HiredDate = hired,
                        ResignationDate = resigned,
                        InsertedDate = hired,
                    },
                };
                staff.Add(employee);
            }
        }
        dataContext.Employees.AddRange(staff);
        return staff;
    }

    private readonly Dictionary<int, int> _studentSequence = [];

    private void AddStudent(Programme programme, Department department, Level level, int intake, DateTime semesterStart, List<Course> currentCourses)
    {
        var sequence = _studentSequence[intake] = _studentSequence.GetValueOrDefault(intake) + 1;
        var studentId = $"STU-{intake}-{sequence:D4}";

        var female = _random.NextDouble() < programme.FemaleShare;
        var (first, middle, last) = RandomName(female);
        var admitted = new DateTime(intake, 2, 8).AddDays(_random.Next(0, 12));
        var born = new DateTime(intake - 19, 1, 1).AddDays(_random.Next(0, 730));
        var guardianIsMother = _random.NextDouble() < 0.18;

        var student = new Student
        {
            StudentId = studentId,
            FirstName = first,
            MiddleName = middle,
            LastName = last,
            Department = department,
            Level = level,
            InsertedDate = admitted,
            PersonalInfo = new StudentPersonalInfo
            {
                StudentId = studentId,
                Gender = female ? "Female" : "Male",
                DateOfBirth = born,
                CitizenshipNumber = $"{_random.Next(1, 78):D2}-{_random.Next(1, 4):D2}-{born.Year % 100 + 56:D2}-{_random.Next(10000, 99999)}",
                Email = UniqueEmail(first, last, "student.mtech.edu.np"),
                Address = Pick(Localities),
                ContactNumber = MobileNumber(),
                GuardianName = guardianIsMother
                    ? $"{Pick(FemaleNames)} {last}"
                    : $"{Pick(MaleNames)} {Pick(MaleMiddleNames)} {last}",
                GuardianRelationship = guardianIsMother ? "Mother" : "Father",
                GuardianContactNumber = MobileNumber(),
                AdmissionDate = admitted,
                InsertedDate = admitted,
            },
        };
        dataContext.Students.Add(student);

        foreach (var course in currentCourses)
        {
            dataContext.Enrollments.Add(new Enrollment
            {
                Student = student,
                Course = course,
                EnrolledDate = semesterStart.AddDays(_random.Next(-6, 3)),
                InsertedDate = semesterStart,
            });
        }
    }

    private (string First, string? Middle, string Last) RandomName(bool female)
    {
        var first = Pick(female ? FemaleNames : MaleNames);
        string? middle = _random.NextDouble() < 0.22 ? Pick(female ? FemaleMiddleNames : MaleMiddleNames) : null;
        return (first, middle, Pick(Surnames));
    }

    private string UniqueEmail(string first, string last, string domain)
    {
        var local = $"{first}.{last}".ToLowerInvariant();
        var email = $"{local}@{domain}";
        for (var n = 2; !_usedEmails.Add(email); n++)
            email = $"{local}{n}@{domain}";
        return email;
    }

    private string MobileNumber() => $"98{_random.Next(0, 6)}{_random.Next(1000000, 9999999)}";

    private DateTime RandomDate(DateTime from, DateTime to) =>
        to <= from ? from : from.AddDays(_random.Next(0, (int)(to - from).TotalDays));

    private T Pick<T>(T[] items) => items[_random.Next(items.Length)];
}
