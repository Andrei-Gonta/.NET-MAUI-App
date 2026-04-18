using CarMeetApp.Models;

namespace CarMeetApp.Services;

public static class UserSession
{
    public static string Email { get; private set; } = string.Empty;
    public static string FirebaseIdToken { get; private set; } = string.Empty;

    public static UserRole Role { get; private set; } = UserRole.Guest;

    public static bool IsLoggedIn => Role != UserRole.Guest;

    public static void LoginAs(string email, UserRole role, string firebaseIdToken = "")
    {
        Email = email;
        Role = role;
        FirebaseIdToken = firebaseIdToken;
    }

    public static void Logout()
    {
        Email = string.Empty;
        Role = UserRole.Guest;
        FirebaseIdToken = string.Empty;
    }
}
