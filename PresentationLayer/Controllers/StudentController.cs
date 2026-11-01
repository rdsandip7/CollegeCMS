using BusinessLayer;
using CoreLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[Authorize]
public class StudentController(
    StudentService studentService,
    DepartmentService departmentService,
    LevelService levelService,
    CourseService courseService) : AppController
{
    public async Task<IActionResult> Index(string? search, string? sort, int page = 1) =>
        View(await studentService.GetStudentsAsync(BuildListQuery(search, sort, page)));

    public async Task<IActionResult> Details(string id)
    {
        var student = await studentService.GetStudentByIdAsync(id);
        if (student is null)
            return NotFound();

        // Group the course picker by department, the student's own department first.
        var enrolledIds = student.Enrollments.Select(e => e.CourseId).ToHashSet();
        var groups = new Dictionary<string, SelectListGroup>();
        ViewBag.AvailableCourses = (await courseService.GetAllCoursesAsync())
            .Where(c => !enrolledIds.Contains(c.CourseId))
            .OrderBy(c => c.DepartmentId == student.DepartmentId ? 0 : 1)
            .ThenBy(c => c.Department?.DepartmentName)
            .Select(c =>
            {
                var department = c.Department?.DepartmentName ?? "Other";
                if (!groups.TryGetValue(department, out var group))
                    groups[department] = group = new SelectListGroup { Name = department };
                return new SelectListItem($"{c.CourseName} ({c.Level?.DisplayName})", c.CourseId.ToString()) { Group = group };
            })
            .ToList();
        return View(student);
    }

    public async Task<IActionResult> Create() => View(await WithLookupsAsync(new StudentViewModel()));

    [HttpPost]
    public async Task<IActionResult> Create(StudentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await WithLookupsAsync(model));

        var studentId = await studentService.SaveStudentAsync(model.ToEntity());
        Flash($"Student {model.FirstName} {model.LastName} was admitted with ID {studentId}.");
        return RedirectToAction(nameof(Details), new { id = studentId });
    }

    public async Task<IActionResult> Edit(string id)
    {
        var student = await studentService.GetStudentByIdAsync(id);
        return student is null ? NotFound() : View(await WithLookupsAsync(StudentViewModel.From(student)));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, StudentViewModel model)
    {
        model.StudentId = id;
        if (!ModelState.IsValid)
            return View(await WithLookupsAsync(model));

        var result = await studentService.UpdateStudentAsync(model.ToEntity());
        if (!result.Succeeded)
            return RedirectWithResult(result, string.Empty);

        Flash($"Student {model.FirstName} {model.LastName} was updated.");
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(string id) =>
        RedirectWithResult(await studentService.DeleteStudentAsync(id), "Student deleted.");

    [HttpPost]
    public async Task<IActionResult> Enroll(string id, int courseId)
    {
        var result = await studentService.EnrollAsync(id, courseId);
        Flash(result.Succeeded ? "Enrolled in course." : result.Error!, result.Succeeded ? "success" : "danger");
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Unenroll(string id, int enrollmentId)
    {
        var result = await studentService.UnenrollAsync(enrollmentId);
        Flash(result.Succeeded ? "Enrollment removed." : result.Error!, result.Succeeded ? "success" : "danger");
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task<StudentViewModel> WithLookupsAsync(StudentViewModel model)
    {
        model.Departments = new SelectList(await departmentService.GetAllDepartmentsAsync(), nameof(Department.DepartmentId), nameof(Department.DepartmentName));
        model.Levels = new SelectList(await levelService.GetAllLevelsAsync(), nameof(Level.LevelId), nameof(Level.DisplayName));
        return model;
    }
}
