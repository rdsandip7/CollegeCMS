using DataAccessLayer;

namespace PresentationLayer.Models;

public record DashboardViewModel(CollegeStatistics Statistics, IReadOnlyList<DepartmentHeadcount> Departments);

public record ErrorViewModel(int StatusCode, string? RequestId);
