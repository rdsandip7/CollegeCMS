namespace CoreLayer;

public class User
{
    public int UserId { get; set; }
    public string Role { get; set; } = Roles.Staff;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime? InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string Staff = "Staff";
}
