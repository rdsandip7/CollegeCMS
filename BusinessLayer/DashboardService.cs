using DataAccessLayer;

namespace BusinessLayer;

public class DashboardService(StatisticsCRUD statisticsTableActions)
{
    public Task<CollegeStatistics> GetStatisticsAsync() => statisticsTableActions.GetStatisticsAsync();

    public Task<List<DepartmentSummary>> GetDepartmentSummariesAsync() => statisticsTableActions.GetDepartmentSummariesAsync();

    public Task<List<IntakeCount>> GetIntakeCountsAsync() => statisticsTableActions.GetIntakeCountsAsync();

    public Task<List<PositionCount>> GetStaffByPositionAsync() => statisticsTableActions.GetStaffByPositionAsync();

    public Task<List<RecentStudent>> GetRecentAdmissionsAsync(int count = 6) => statisticsTableActions.GetRecentAdmissionsAsync(count);

    public Task<List<RecentHire>> GetRecentHiresAsync(int count = 5) => statisticsTableActions.GetRecentHiresAsync(count);
}
