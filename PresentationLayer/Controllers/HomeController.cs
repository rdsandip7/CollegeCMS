using System.Diagnostics;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class HomeController(DashboardService dashboardService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var statistics = await dashboardService.GetStatisticsAsync();
        var programmes = (await dashboardService.GetDepartmentSummariesAsync()).Where(d => d.Students > 0).ToList();
        return View(new HomeViewModel(statistics, programmes));
    }

    [Authorize]
    public async Task<IActionResult> Dashboard() => View(new DashboardViewModel(
        await dashboardService.GetStatisticsAsync(),
        await dashboardService.GetDepartmentSummariesAsync(),
        await dashboardService.GetIntakeCountsAsync(),
        await dashboardService.GetStaffByPositionAsync(),
        await dashboardService.GetRecentAdmissionsAsync(),
        await dashboardService.GetRecentHiresAsync()));

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel(500, Activity.Current?.Id ?? HttpContext.TraceIdentifier));

    [ActionName("StatusCode")]
    public IActionResult StatusCodePage(int code) =>
        View("Error", new ErrorViewModel(code, HttpContext.TraceIdentifier));
}
