using DataAccessLayer;

namespace BusinessLayer;

public class DashboardService(StatisticsCRUD statisticsTableActions)
{
    public Task<CollegeStatistics> GetStatisticsAsync() => statisticsTableActions.GetStatisticsAsync();

    public Task<List<DepartmentHeadcount>> GetDepartmentHeadcountsAsync() =>
        statisticsTableActions.GetDepartmentHeadcountsAsync();
}
