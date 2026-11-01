using BusinessLayer;
using CoreLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[Authorize]
public class EmployeeController(EmployeeService employeeService, DepartmentService departmentService) : AppController
{
    public async Task<IActionResult> Index(string? search, string? sort, int page = 1) =>
        View(await employeeService.GetEmployeesAsync(BuildListQuery(search, sort, page)));

    public async Task<IActionResult> Details(int id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        return employee is null ? NotFound() : View(employee);
    }

    public async Task<IActionResult> Create() => View(await WithLookupsAsync(new EmployeeViewModel()));

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await WithLookupsAsync(model));

        var result = await employeeService.SaveEmployeeAsync(model.ToEntity());
        if (!result.Succeeded)
        {
            ModelState.AddModelError(nameof(model.Email), result.Error!);
            return View(await WithLookupsAsync(model));
        }

        Flash($"Employee {model.FirstName} {model.LastName} was added.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        return employee is null ? NotFound() : View(await WithLookupsAsync(EmployeeViewModel.From(employee)));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
    {
        model.EmployeeId = id;
        if (!ModelState.IsValid)
            return View(await WithLookupsAsync(model));

        var result = await employeeService.UpdateEmployeeAsync(model.ToEntity());
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(await WithLookupsAsync(model));
        }

        Flash($"Employee {model.FirstName} {model.LastName} was updated.");
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id) =>
        RedirectWithResult(await employeeService.DeleteEmployeeAsync(id), "Employee deleted.");

    private async Task<EmployeeViewModel> WithLookupsAsync(EmployeeViewModel model)
    {
        model.Departments = new SelectList(await departmentService.GetAllDepartmentsAsync(), nameof(Department.DepartmentId), nameof(Department.DepartmentName));
        return model;
    }
}
