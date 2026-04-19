using CarMeetApp.Services;
using CarMeetApp.Models;

namespace CarMeetApp.Pages;

public partial class UserProfilePage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    private User? _currentUser;

    public string AvatarPhotoPath { get; private set; } = string.Empty;
    public bool HasAvatar => !string.IsNullOrWhiteSpace(AvatarPhotoPath) && File.Exists(AvatarPhotoPath);
    public bool HasNoAvatar => !HasAvatar;
    public string FullNameDisplay { get; private set; } = "Name: empty";
    public string LocationDisplay { get; private set; } = "Location: N/A";
    public string AgeDisplay { get; private set; } = "Age: N/A";
    public string SocialLinksDisplay { get; private set; } = "Social links: N/A";
    public string PhoneNumberDisplay { get; private set; } = "Phone number: empty";
    public string CarDisplay { get; private set; } = "Car: N/A";
    public string ShortDescriptionDisplay { get; private set; } = "Short description: N/A";

    public UserProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProfileAsync();
    }

    private async Task LoadProfileAsync()
    {
        if (string.IsNullOrWhiteSpace(UserSession.Email))
        {
            return;
        }

        _currentUser = await _databaseService.GetUserByEmailAsync(UserSession.Email);
        if (_currentUser is null)
        {
            _currentUser = await _databaseService.AddUserAsync(new User
            {
                Email = UserSession.Email,
                FullName = "empty",
                Location = "N/A",
                Age = "N/A",
                SocialLinks = "N/A",
                PhoneNumber = "empty",
                AvatarPhotoPath = string.Empty,
                ShortDescription = "N/A",
                CarDescription = "N/A",
                Role = UserSession.Role
            });
        }

        AvatarPhotoPath = _currentUser.AvatarPhotoPath ?? string.Empty;
        FullNameDisplay = $"Name: {Normalize(_currentUser.FullName, "empty")}";
        LocationDisplay = $"Location: {Normalize(_currentUser.Location, "N/A")}";
        AgeDisplay = $"Age: {Normalize(_currentUser.Age, "N/A")}";
        SocialLinksDisplay = $"Social links: {Normalize(_currentUser.SocialLinks, "N/A")}";
        PhoneNumberDisplay = $"Phone number: {Normalize(_currentUser.PhoneNumber, "empty")}";
        CarDisplay = $"Car: {Normalize(_currentUser.CarDescription, "N/A")}";
        ShortDescriptionDisplay = $"Short description: {Normalize(_currentUser.ShortDescription, "N/A")}";

        OnPropertyChanged(nameof(AvatarPhotoPath));
        OnPropertyChanged(nameof(HasAvatar));
        OnPropertyChanged(nameof(HasNoAvatar));
        OnPropertyChanged(nameof(FullNameDisplay));
        OnPropertyChanged(nameof(LocationDisplay));
        OnPropertyChanged(nameof(AgeDisplay));
        OnPropertyChanged(nameof(SocialLinksDisplay));
        OnPropertyChanged(nameof(PhoneNumberDisplay));
        OnPropertyChanged(nameof(CarDisplay));
        OnPropertyChanged(nameof(ShortDescriptionDisplay));
    }

    private static string Normalize(string? value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value;

    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditUserProfilePage());
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }
}
