using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class LoginPage : ContentPage, INotifyPropertyChanged
{
    private string _email = "email@user.com";
    private string _password = "password123";
    private string _loginStatus = string.Empty;

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

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            LoginStatus = "Please enter both email and password.";
            return;
        }

        if (Email.ToLower() == "email@admin.com")
        {
            UserSession.LoginAs(Email, UserRole.Admin);
            LoginStatus = "Logged in as Admin.";
        }
        else if (Email.ToLower() == "email@user.com")
        {
            UserSession.LoginAs(Email, UserRole.RegularUser);
            LoginStatus = "Logged in as Regular User.";
        }
        else
        {
            LoginStatus = "Use email@user.com or email@admin.com.";
            return;
        }

        Application.Current!.MainPage = new AppShell();
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
