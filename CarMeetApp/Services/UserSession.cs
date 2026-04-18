using CarMeetApp.Models;

namespace CarMeetApp.Services;

public static class UserSession
{
    public static string Email { get; private set; } = string.Empty;

    public static UserRole Role { get; private set; } = UserRole.Guest;

    public static bool IsLoggedIn => Role != UserRole.Guest;

    public static void LoginAs(string email, UserRole role)
    {
        Email = email;
        Role = role;
    }

    public static void Logout()
    {
        Email = string.Empty;
        Role = UserRole.Guest;
    }
}
