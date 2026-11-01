using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer;

public class DepartmentService(DepartmentCRUD departmentTableActions)
{
    public Task<PagedResult<Department>> GetDepartmentsAsync(ListQuery query) =>
        departmentTableActions.GetDepartmentsAsync(query);

    public Task<List<Department>> GetAllDepartmentsAsync() => departmentTableActions.GetAllDepartmentsAsync();

    public Task<Department?> GetDepartmentByIdAsync(int departmentId) =>
        departmentTableActions.GetDepartmentByIdAsync(departmentId);

    public async Task<OperationResult> SaveDepartmentAsync(Department department)
    {
        if (await departmentTableActions.DepartmentNameExistsAsync(department.DepartmentName))
            return OperationResult.Fail($"A department named \"{department.DepartmentName}\" already exists.");

        department.InsertedDate = DateTime.Now;
        await departmentTableActions.SaveDepartmentAsync(department);
        return OperationResult.Success;
    }

    public async Task<OperationResult> UpdateDepartmentAsync(Department changes)
    {
        var department = await departmentTableActions.GetDepartmentByIdAsync(changes.DepartmentId);
        if (department is null)
            return OperationResult.Fail("Department not found.");
        if (await departmentTableActions.DepartmentNameExistsAsync(changes.DepartmentName, changes.DepartmentId))
            return OperationResult.Fail($"A department named \"{changes.DepartmentName}\" already exists.");

        department.DepartmentName = changes.DepartmentName;
        department.Description = changes.Description;
        department.DepartmentHead = changes.DepartmentHead;
        department.EstablishedDate = changes.EstablishedDate;
        department.UpdatedDate = DateTime.Now;
        await departmentTableActions.UpdateDepartmentAsync(department);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteDepartmentAsync(int departmentId)
    {
        var department = await departmentTableActions.GetDepartmentByIdAsync(departmentId);
        if (department is null)
            return OperationResult.Fail("Department not found.");
        if (await departmentTableActions.HasDependentsAsync(departmentId))
            return OperationResult.Fail($"\"{department.DepartmentName}\" still has employees, students or courses. Move or remove them first.");

        await departmentTableActions.RemoveDepartmentAsync(department);
        return OperationResult.Success;
    }
}
