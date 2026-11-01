using BusinessLayer;
using CoreLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[Authorize]
public class DepartmentController(DepartmentService departmentService) : AppController
{
    public async Task<IActionResult> Index(string? search, string? sort, int page = 1) =>
        View(await departmentService.GetDepartmentsAsync(BuildListQuery(search, sort, page)));

    public IActionResult Create() => View(new DepartmentViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(DepartmentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await departmentService.SaveDepartmentAsync(model.ToEntity());
        if (!result.Succeeded)
        {
            ModelState.AddModelError(nameof(model.DepartmentName), result.Error!);
            return View(model);
        }

        Flash($"Department \"{model.DepartmentName}\" was created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var department = await departmentService.GetDepartmentByIdAsync(id);
        return department is null ? NotFound() : View(DepartmentViewModel.From(department));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, DepartmentViewModel model)
    {
        model.DeptId = id;
        if (!ModelState.IsValid)
            return View(model);

        var result = await departmentService.UpdateDepartmentAsync(model.ToEntity());
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(model);
        }

        Flash($"Department \"{model.DepartmentName}\" was updated.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id) =>
        RedirectWithResult(await departmentService.DeleteDepartmentAsync(id), "Department deleted.");
}
