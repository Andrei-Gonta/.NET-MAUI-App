using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class EventSignUpPage : ContentPage, INotifyPropertyChanged
{
    private string? _selectedEventName;
    private string _fullName = string.Empty;
    private string _phoneNumber = string.Empty;
    private string? _selectedBrand;
    private string? _selectedModel;
    private string? _selectedGenerationName;
    private string _horsepowerText = "Horsepower: -";
    private string _engineCapacityText = "Engine capacity: -";
    private string _signUpStatus = string.Empty;
    private readonly CarDataService _carDataService = new();
    private readonly DatabaseService _databaseService = new();
    private List<CarGenerationOption> _generationOptions = [];
    private List<EventItem> _events = [];

    public List<string> EventNames { get; private set; } = [];
    public ObservableCollection<string> CarBrands { get; } = [];
    public ObservableCollection<string> CarModels { get; } = [];
    public ObservableCollection<string> CarGenerations { get; } = [];

    public new event PropertyChangedEventHandler? PropertyChanged;

    public string? SelectedEventName
    {
        get => _selectedEventName;
        set => SetField(ref _selectedEventName, value);
    }

    public string FullName
    {
        get => _fullName;
        set => SetField(ref _fullName, value);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetField(ref _phoneNumber, value);
    }

    public string? SelectedBrand
    {
        get => _selectedBrand;
        set => SetField(ref _selectedBrand, value);
    }

    public string? SelectedModel
    {
        get => _selectedModel;
        set => SetField(ref _selectedModel, value);
    }

    public string? SelectedGenerationName
    {
        get => _selectedGenerationName;
        set => SetField(ref _selectedGenerationName, value);
    }

    public string HorsepowerText
    {
        get => _horsepowerText;
        set => SetField(ref _horsepowerText, value);
    }

    public string EngineCapacityText
    {
        get => _engineCapacityText;
        set => SetField(ref _engineCapacityText, value);
    }

    public string SignUpStatus
    {
        get => _signUpStatus;
        set => SetField(ref _signUpStatus, value);
    }

    public EventSignUpPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            _events = await _databaseService.GetEventsAsync();
            EventNames = _events.Select(x => x.Title).ToList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EventNames)));
            await LoadBrandsAsync();
        }
        catch (Exception ex)
        {
            SignUpStatus = $"Error loading data: {ex.Message}";
        }
    }

    private async Task LoadBrandsAsync()
    {
        var brands = await _carDataService.GetBrandsAsync();
        CarBrands.Clear();
        foreach (var brand in brands)
        {
            CarBrands.Add(brand);
        }
    }

    private async void OnBrandChanged(object? sender, EventArgs e)
    {
        SelectedModel = null;
        SelectedGenerationName = null;
        CarModels.Clear();
        CarGenerations.Clear();
        ResetSpecs();

        if (string.IsNullOrWhiteSpace(SelectedBrand))
        {
            return;
        }

        var models = await _carDataService.GetModelsAsync(SelectedBrand);
        foreach (var model in models)
        {
            CarModels.Add(model);
        }
    }

    private async void OnModelChanged(object? sender, EventArgs e)
    {
        SelectedGenerationName = null;
        CarGenerations.Clear();
        ResetSpecs();

        if (string.IsNullOrWhiteSpace(SelectedBrand) || string.IsNullOrWhiteSpace(SelectedModel))
        {
            return;
        }

        _generationOptions = await _carDataService.GetGenerationsAsync(SelectedBrand, SelectedModel);
        foreach (var generation in _generationOptions)
        {
            CarGenerations.Add(generation.Generation);
        }
    }

    private void OnGenerationChanged(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SelectedGenerationName))
        {
            ResetSpecs();
            return;
        }

        var selectedGeneration = _generationOptions.FirstOrDefault(x => x.Generation == SelectedGenerationName);
        HorsepowerText = selectedGeneration?.HorsepowerHp is int hp ? $"Horsepower: {hp} HP" : "Horsepower: Not available";
        EngineCapacityText = selectedGeneration?.EngineSizeLiters is double size
            ? $"Engine capacity: {size:0.0} L"
            : "Engine capacity: Not available";
    }

    private void ResetSpecs()
    {
        HorsepowerText = "Horsepower: -";
        EngineCapacityText = "Engine capacity: -";
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (UserSession.Role != UserRole.RegularUser)
        {
            SignUpStatus = "Only regular users can join events.";
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedEventName) ||
            string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(PhoneNumber) ||
            string.IsNullOrWhiteSpace(SelectedBrand) ||
            string.IsNullOrWhiteSpace(SelectedModel) ||
            string.IsNullOrWhiteSpace(SelectedGenerationName))
        {
            SignUpStatus = "Please complete event, name, phone, brand, model, and generation.";
            return;
        }

        try
        {
            // Get the selected event
            var selectedEvent = _events.FirstOrDefault(x => x.Title.ToLower() == SelectedEventName.ToLower());
            if (selectedEvent == null)
            {
                SignUpStatus = "Event not found.";
                return;
            }

            // Get or create user
            var user = await _databaseService.GetUserByEmailAsync(UserSession.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = UserSession.Email,
                    FullName = FullName,
                    PhoneNumber = PhoneNumber,
                    SelectedBrand = SelectedBrand,
                    SelectedModel = SelectedModel,
                    SelectedGeneration = SelectedGenerationName,
                    HorsepowerHp = _generationOptions.FirstOrDefault(x => x.Generation == SelectedGenerationName)?.HorsepowerHp,
                    EngineSizeLiters = _generationOptions.FirstOrDefault(x => x.Generation == SelectedGenerationName)?.EngineSizeLiters,
                    Role = UserSession.Role
                };
                user = await _databaseService.AddUserAsync(user);
            }
            else
            {
                // Update existing user with new information
                user.FullName = FullName;
                user.PhoneNumber = PhoneNumber;
                user.SelectedBrand = SelectedBrand;
                user.SelectedModel = SelectedModel;
                user.SelectedGeneration = SelectedGenerationName;
                user.HorsepowerHp = _generationOptions.FirstOrDefault(x => x.Generation == SelectedGenerationName)?.HorsepowerHp;
                user.EngineSizeLiters = _generationOptions.FirstOrDefault(x => x.Generation == SelectedGenerationName)?.EngineSizeLiters;
                await _databaseService.UpdateUserAsync(user);
            }

            // Sign up for event
            var selectedGeneration = _generationOptions.FirstOrDefault(x => x.Generation == SelectedGenerationName);
            var success = await _databaseService.SignUpForEventAsync(
                UserSession.Email,
                selectedEvent.Id,
                SelectedBrand,
                SelectedModel,
                SelectedGenerationName,
                selectedGeneration?.HorsepowerHp,
                selectedGeneration?.EngineSizeLiters
            );

            if (success)
            {
                SignUpStatus = $"{FullName} joined {SelectedEventName} with {SelectedBrand} {SelectedModel} ({SelectedGenerationName}).";
            }
            else
            {
                SignUpStatus = "You are already signed up for this event.";
            }
        }
        catch (Exception ex)
        {
            SignUpStatus = $"Error signing up: {ex.Message}";
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
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
