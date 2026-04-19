using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class EditUserProfilePage : ContentPage, INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService = new();
    private User? _currentUser;
    private string _fullName = "empty";
    private string _location = "N/A";
    private string _age = "N/A";
    private string _socialLinks = "N/A";
    private string _phoneNumber = "empty";
    private string _avatarPhotoPath = string.Empty;
    private string _shortDescription = "N/A";
    private string _carDescription = "N/A";
    private string _statusMessage = string.Empty;

    public new event PropertyChangedEventHandler? PropertyChanged;

    public string FullName
    {
        get => _fullName;
        set => SetField(ref _fullName, string.IsNullOrWhiteSpace(value) ? "empty" : value);
    }

    public string Location
    {
        get => _location;
        set => SetField(ref _location, string.IsNullOrWhiteSpace(value) ? "N/A" : value);
    }

    public string Age
    {
        get => _age;
        set => SetField(ref _age, string.IsNullOrWhiteSpace(value) ? "N/A" : value);
    }

    public string SocialLinks
    {
        get => _socialLinks;
        set => SetField(ref _socialLinks, string.IsNullOrWhiteSpace(value) ? "N/A" : value);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetField(ref _phoneNumber, string.IsNullOrWhiteSpace(value) ? "empty" : value);
    }

    public string AvatarPhotoPath
    {
        get => _avatarPhotoPath;
        set
        {
            if (!SetField(ref _avatarPhotoPath, value))
            {
                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasAvatar)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasNoAvatar)));
        }
    }

    public string ShortDescription
    {
        get => _shortDescription;
        set => SetField(ref _shortDescription, string.IsNullOrWhiteSpace(value) ? "N/A" : value);
    }

    public string CarDescription
    {
        get => _carDescription;
        set => SetField(ref _carDescription, string.IsNullOrWhiteSpace(value) ? "N/A" : value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public bool HasAvatar => !string.IsNullOrWhiteSpace(AvatarPhotoPath) && File.Exists(AvatarPhotoPath);
    public bool HasNoAvatar => !HasAvatar;

    public EditUserProfilePage()
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
            StatusMessage = "Please login to edit profile.";
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

        FullName = string.IsNullOrWhiteSpace(_currentUser.FullName) ? "empty" : _currentUser.FullName;
        Location = string.IsNullOrWhiteSpace(_currentUser.Location) ? "N/A" : _currentUser.Location;
        Age = string.IsNullOrWhiteSpace(_currentUser.Age) ? "N/A" : _currentUser.Age;
        SocialLinks = string.IsNullOrWhiteSpace(_currentUser.SocialLinks) ? "N/A" : _currentUser.SocialLinks;
        PhoneNumber = string.IsNullOrWhiteSpace(_currentUser.PhoneNumber) ? "empty" : _currentUser.PhoneNumber;
        AvatarPhotoPath = _currentUser.AvatarPhotoPath ?? string.Empty;
        ShortDescription = string.IsNullOrWhiteSpace(_currentUser.ShortDescription) ? "N/A" : _currentUser.ShortDescription;
        CarDescription = string.IsNullOrWhiteSpace(_currentUser.CarDescription) ? "N/A" : _currentUser.CarDescription;
        StatusMessage = string.Empty;
    }

    private async void OnChooseAvatarClicked(object sender, EventArgs e)
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Choose avatar photo",
                FileTypes = FilePickerFileType.Images
            });

            if (file is null)
            {
                return;
            }

            AvatarPhotoPath = await SaveAvatarLocallyAsync(file);
            StatusMessage = "Avatar selected.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Could not pick avatar: {ex.Message}";
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_currentUser is null)
        {
            StatusMessage = "Profile not loaded.";
            return;
        }

        _currentUser.FullName = FullName;
        _currentUser.Location = Location;
        _currentUser.Age = Age;
        _currentUser.SocialLinks = SocialLinks;
        _currentUser.PhoneNumber = PhoneNumber;
        _currentUser.AvatarPhotoPath = AvatarPhotoPath;
        _currentUser.ShortDescription = ShortDescription;
        _currentUser.CarDescription = CarDescription;

        var saved = await _databaseService.UpdateUserAsync(_currentUser);
        StatusMessage = saved ? "Profile updated successfully." : "No changes were saved.";
        if (saved)
        {
            await Navigation.PopAsync();
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }

    private static async Task<string> SaveAvatarLocallyAsync(FileResult file)
    {
        var avatarsDir = Path.Combine(FileSystem.AppDataDirectory, "avatars");
        Directory.CreateDirectory(avatarsDir);

        var extension = Path.GetExtension(file.FileName);
        var safeExtension = string.IsNullOrWhiteSpace(extension) ? ".jpg" : extension;
        var targetPath = Path.Combine(avatarsDir, $"{Guid.NewGuid()}{safeExtension}");

        await using var source = await file.OpenReadAsync();
        await using var destination = File.Create(targetPath);
        await source.CopyToAsync(destination);
        return targetPath;
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
