using System.Linq.Expressions;
using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class EmployeeCRUD(DataContext dataContext)
{
    private static readonly Dictionary<string, Expression<Func<Employee, object?>>> SortColumns = new()
    {
        ["name"] = x => x.FirstName,
        ["email"] = x => x.Email,
        ["department"] = x => x.Department!.DepartmentName,
        ["position"] = x => x.PersonalInfo.Position,
        ["hired"] = x => x.PersonalInfo.HiredDate,
    };

    //Create Employee (the personal info row is saved with it)
    public async Task SaveEmployeeAsync(Employee employee)
    {
        dataContext.Employees.Add(employee);
        await dataContext.SaveChangesAsync();
    }

    //Read Employee
    public Task<Employee?> GetEmployeeByIdAsync(int employeeId) =>
        dataContext.Employees
            .Include(x => x.Department)
            .Include(x => x.PersonalInfo)
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

    public Task<Employee?> GetEmployeeByEmailAsync(string email) =>
        dataContext.Employees.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

    public Task<bool> EmailExistsAsync(string email, int? exceptId = null) =>
        dataContext.Employees.AnyAsync(x => x.Email.ToLower() == email.ToLower() && x.EmployeeId != exceptId);

    public Task<PagedResult<Employee>> GetEmployeesAsync(ListQuery query)
    {
        IQueryable<Employee> employees = dataContext.Employees.AsNoTracking()
            .Include(x => x.Department)
            .Include(x => x.PersonalInfo);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim().ToLower();
            employees = employees.Where(x => x.FirstName.ToLower().Contains(s)
                || (x.MiddleName != null && x.MiddleName.ToLower().Contains(s))
                || x.LastName.ToLower().Contains(s)
                || x.Email.ToLower().Contains(s)
                || x.Department!.DepartmentName.ToLower().Contains(s)
                || (x.PersonalInfo.Position != null && x.PersonalInfo.Position.ToLower().Contains(s)));
        }

        return employees.ApplySort(query, SortColumns).ToPagedResultAsync(query);
    }

    //Update Employee
    public async Task UpdateEmployeeAsync(Employee employee)
    {
        dataContext.Employees.Update(employee);
        await dataContext.SaveChangesAsync();
    }

    //Delete Employee (personal info is removed by cascade)
    public async Task RemoveEmployeeAsync(Employee employee)
    {
        dataContext.Employees.Remove(employee);
        await dataContext.SaveChangesAsync();
    }
}
