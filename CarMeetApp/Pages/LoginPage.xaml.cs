using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class LoginPage : ContentPage, INotifyPropertyChanged
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _loginStatus = string.Empty;
    private bool _isSigningIn;
    private readonly FirebaseAuthService _firebaseAuthService = new();

    public new event PropertyChangedEventHandler? PropertyChanged;

    public string Email
    {
        get => _email;
        set => SetField(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public string LoginStatus
    {
        get => _loginStatus;
        set => SetField(ref _loginStatus, value);
    }

    public bool IsSigningIn
    {
        get => _isSigningIn;
        set
        {
            if (!SetField(ref _isSigningIn, value))
            {
                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotBusy)));
        }
    }

    public bool IsNotBusy => !IsSigningIn;

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (IsSigningIn)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            LoginStatus = "Please enter both email and password.";
            return;
        }

        IsSigningIn = true;
        LoginStatus = "Signing in...";

        var authResult = await _firebaseAuthService.SignInAsync(Email.Trim(), Password);
        if (!authResult.IsSuccess)
        {
            LoginStatus = authResult.ErrorMessage;
            IsSigningIn = false;
            return;
        }

        var signedInEmail = authResult.Email;
        var role = ResolveRoleFromEmail(signedInEmail);
        if (role == UserRole.Guest)
        {
            LoginStatus = "Signed in, but email must end with @email.com or @admin.com.";
            IsSigningIn = false;
            return;
        }

        UserSession.LoginAs(signedInEmail, role, authResult.IdToken);
        LoginStatus = role == UserRole.Admin ? "Logged in as Admin." : "Logged in as Regular User.";
        Application.Current!.MainPage = new AppShell();
    }

    private static UserRole ResolveRoleFromEmail(string email)
    {
        if (email.EndsWith("@admin.com", StringComparison.OrdinalIgnoreCase))
        {
            return UserRole.Admin;
        }

        if (email.EndsWith("@email.com", StringComparison.OrdinalIgnoreCase))
        {
            return UserRole.RegularUser;
        }

        return UserRole.Guest;
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
