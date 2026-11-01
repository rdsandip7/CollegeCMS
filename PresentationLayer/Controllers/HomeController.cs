using System.Diagnostics;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class HomeController(DashboardService dashboardService) : Controller
{
    public IActionResult Index() => View();

    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        var statistics = await dashboardService.GetStatisticsAsync();
        var departments = await dashboardService.GetDepartmentHeadcountsAsync();
        return View(new DashboardViewModel(statistics, departments));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel(500, Activity.Current?.Id ?? HttpContext.TraceIdentifier));

    [ActionName("StatusCode")]
    public IActionResult StatusCodePage(int code) =>
        View("Error", new ErrorViewModel(code, HttpContext.TraceIdentifier));
}
