using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer;

public class EmployeeService(EmployeeCRUD employeeTableActions)
{
    public Task<PagedResult<Employee>> GetEmployeesAsync(ListQuery query) => employeeTableActions.GetEmployeesAsync(query);

    public Task<Employee?> GetEmployeeByIdAsync(int employeeId) => employeeTableActions.GetEmployeeByIdAsync(employeeId);

    public async Task<OperationResult> SaveEmployeeAsync(Employee employee)
    {
        if (await employeeTableActions.EmailExistsAsync(employee.Email))
            return OperationResult.Fail($"An employee with email {employee.Email} already exists.");

        employee.InsertedDate = DateTime.Now;
        employee.PersonalInfo.InsertedDate = DateTime.Now;
        await employeeTableActions.SaveEmployeeAsync(employee);
        return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateEmployeeAsync(Employee changes)
    {
        var employee = await employeeTableActions.GetEmployeeByIdAsync(changes.EmployeeId);
        if (employee is null)
            return OperationResult.Fail("Employee not found.");
        if (await employeeTableActions.EmailExistsAsync(changes.Email, changes.EmployeeId))
            return OperationResult.Fail($"An employee with email {changes.Email} already exists.");

        employee.FirstName = changes.FirstName;
        employee.MiddleName = changes.MiddleName;
        employee.LastName = changes.LastName;
        employee.Email = changes.Email;
        employee.DepartmentId = changes.DepartmentId;
        employee.UpdatedDate = DateTime.Now;

        var info = employee.PersonalInfo;
        info.Gender = changes.PersonalInfo.Gender;
        info.Position = changes.PersonalInfo.Position;
        info.Address = changes.PersonalInfo.Address;
        info.Phone = changes.PersonalInfo.Phone;
        info.HiredDate = changes.PersonalInfo.HiredDate;
        info.ResignationDate = changes.PersonalInfo.ResignationDate;
        info.UpdatedDate = DateTime.Now;

        await employeeTableActions.UpdateEmployeeAsync(employee);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteEmployeeAsync(int employeeId)
    {
        var employee = await employeeTableActions.GetEmployeeByIdAsync(employeeId);
        if (employee is null)
            return OperationResult.Fail("Employee not found.");

        await employeeTableActions.RemoveEmployeeAsync(employee);
        return OperationResult.Success;
    }
}
