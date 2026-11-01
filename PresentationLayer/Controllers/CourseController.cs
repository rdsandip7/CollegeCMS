using BusinessLayer;
using CoreLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[Authorize]
public class CourseController(CourseService courseService, DepartmentService departmentService, LevelService levelService) : AppController
{
    public async Task<IActionResult> Index(string? search, string? sort, int page = 1) =>
        View(await courseService.GetCoursesAsync(BuildListQuery(search, sort, page)));

    public async Task<IActionResult> Create() => View(await WithLookupsAsync(new CourseViewModel()));

    [HttpPost]
    public async Task<IActionResult> Create(CourseViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await WithLookupsAsync(model));

        await courseService.SaveCourseAsync(model.ToEntity());
        Flash($"Course \"{model.CourseName}\" was created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var course = await courseService.GetCourseByIdAsync(id);
        return course is null ? NotFound() : View(await WithLookupsAsync(CourseViewModel.From(course)));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CourseViewModel model)
    {
        model.CourseId = id;
        if (!ModelState.IsValid)
            return View(await WithLookupsAsync(model));

        return RedirectWithResult(await courseService.UpdateCourseAsync(model.ToEntity()), $"Course \"{model.CourseName}\" was updated.");
    }

    [HttpPost, Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id) =>
        RedirectWithResult(await courseService.DeleteCourseAsync(id), "Course deleted.");

    private async Task<CourseViewModel> WithLookupsAsync(CourseViewModel model)
    {
        model.Departments = new SelectList(await departmentService.GetAllDepartmentsAsync(), nameof(Department.DepartmentId), nameof(Department.DepartmentName));
        model.Levels = new SelectList(await levelService.GetAllLevelsAsync(), nameof(Level.LevelId), nameof(Level.DisplayName));
        return model;
    }
}
