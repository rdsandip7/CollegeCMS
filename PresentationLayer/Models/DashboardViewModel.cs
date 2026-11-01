using DataAccessLayer;

namespace PresentationLayer.Models;

public record DashboardViewModel(
    CollegeStatistics Statistics,
    IReadOnlyList<DepartmentSummary> Departments,
    IReadOnlyList<IntakeCount> Intakes,
    IReadOnlyList<PositionCount> StaffByPosition,
    IReadOnlyList<RecentStudent> RecentAdmissions,
    IReadOnlyList<RecentHire> RecentHires);

public record HomeViewModel(CollegeStatistics Statistics, IReadOnlyList<DepartmentSummary> Programmes);

public record ErrorViewModel(int StatusCode, string? RequestId);
