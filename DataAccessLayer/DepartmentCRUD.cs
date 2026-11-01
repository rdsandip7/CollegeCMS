using System.Linq.Expressions;
using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class DepartmentCRUD(DataContext dataContext)
{
    private static readonly Dictionary<string, Expression<Func<Department, object?>>> SortColumns = new()
    {
        ["name"] = x => x.DepartmentName,
        ["head"] = x => x.DepartmentHead,
        ["established"] = x => x.EstablishedDate,
    };

    //Create Department
    public async Task SaveDepartmentAsync(Department department)
    {
        dataContext.Departments.Add(department);
        await dataContext.SaveChangesAsync();
    }

    //Read Department
    public Task<Department?> GetDepartmentByIdAsync(int deptId) =>
        dataContext.Departments.FirstOrDefaultAsync(x => x.DepartmentId == deptId);

    public Task<bool> DepartmentNameExistsAsync(string name, int? exceptId = null) =>
        dataContext.Departments.AnyAsync(x => x.DepartmentName.ToLower() == name.ToLower() && x.DepartmentId != exceptId);

    public Task<List<Department>> GetAllDepartmentsAsync() =>
        dataContext.Departments.AsNoTracking().OrderBy(x => x.DepartmentName).ToListAsync();

    public Task<PagedResult<Department>> GetDepartmentsAsync(ListQuery query)
    {
        IQueryable<Department> departments = dataContext.Departments.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim().ToLower();
            departments = departments.Where(x => x.DepartmentName.ToLower().Contains(s)
                || (x.DepartmentHead != null && x.DepartmentHead.ToLower().Contains(s))
                || (x.Description != null && x.Description.ToLower().Contains(s)));
        }

        return departments.ApplySort(query, SortColumns).ToPagedResultAsync(query);
    }

    public async Task<bool> HasDependentsAsync(int deptId) =>
        await dataContext.Employees.AnyAsync(x => x.DepartmentId == deptId)
        || await dataContext.Students.AnyAsync(x => x.DepartmentId == deptId)
        || await dataContext.Courses.AnyAsync(x => x.DepartmentId == deptId);

    //Update Department
    public async Task UpdateDepartmentAsync(Department department)
    {
        dataContext.Departments.Update(department);
        await dataContext.SaveChangesAsync();
    }

    //Delete Department
    public async Task RemoveDepartmentAsync(Department department)
    {
        dataContext.Departments.Remove(department);
        await dataContext.SaveChangesAsync();
    }
}
