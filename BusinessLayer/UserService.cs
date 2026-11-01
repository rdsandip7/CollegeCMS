using CoreLayer;
using DataAccessLayer;

namespace BusinessLayer;

public class UserService(UserCRUD userTableActions, EmployeeCRUD employeeTableActions)
{
    /// <summary>
    /// Creates a staff account. If an employee record has the same email, the account is linked to it.
    /// </summary>
    public async Task<OperationResult> RegisterAsync(string username, string password, string email)
    {
        if (await userTableActions.GetUserByUsernameAsync(username) is not null)
            return OperationResult.Fail("That username is already taken. Please choose another one.");

        var employee = await employeeTableActions.GetEmployeeByEmailAsync(email);
        if (employee?.UserId is not null)
            return OperationResult.Fail("An account is already linked to this email address.");

        var userId = await userTableActions.SaveUserAsync(new User
        {
            Username = username,
            PasswordHash = PasswordHasher.Hash(password),
            Role = Roles.Staff,
            InsertedDate = DateTime.Now,
        });

        if (employee is not null)
        {
            employee.UserId = userId;
            await employeeTableActions.UpdateEmployeeAsync(employee);
        }

        return OperationResult.Success;
    }

    /// <summary>Returns the user when the username and password match, otherwise null.</summary>
    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await userTableActions.GetUserByUsernameAsync(username);
        return user is not null && PasswordHasher.Verify(password, user.PasswordHash) ? user : null;
    }
}
