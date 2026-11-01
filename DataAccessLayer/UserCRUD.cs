using CoreLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class UserCRUD(DataContext dataContext)
{
    //Create User
    public async Task<int> SaveUserAsync(User user)
    {
        dataContext.Users.Add(user);
        await dataContext.SaveChangesAsync();
        return user.UserId;
    }

    //Retrieve User
    public Task<User?> GetUserByUserIdAsync(int userId) =>
        dataContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);

    public Task<User?> GetUserByUsernameAsync(string username) =>
        dataContext.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

    public async Task UpdateUserAsync(User user)
    {
        dataContext.Users.Update(user);
        await dataContext.SaveChangesAsync();
    }
}
