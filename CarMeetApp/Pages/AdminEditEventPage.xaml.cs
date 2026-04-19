using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class AdminEditEventPage : ContentPage, INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService = new();
    private readonly int _eventId;
    private EventItem? _eventItem;
    private string _titleInput = string.Empty;
    private string _locationInput = string.Empty;
    private string _organizerInput = string.Empty;
    private string _descriptionInput = string.Empty;
    private DateTime _eventDate = DateTime.Today;
    private TimeSpan _eventTime = new(18, 0, 0);
    private string _statusMessage = string.Empty;
    private bool _isLoaded;

    public new event PropertyChangedEventHandler? PropertyChanged;

    public string TitleInput
    {
        get => _titleInput;
        set => SetField(ref _titleInput, value);
    }

    public string LocationInput
    {
        get => _locationInput;
        set => SetField(ref _locationInput, value);
    }

    public string OrganizerInput
    {
        get => _organizerInput;
        set => SetField(ref _organizerInput, value);
    }

    public string DescriptionInput
    {
        get => _descriptionInput;
        set => SetField(ref _descriptionInput, value);
    }

    public DateTime EventDate
    {
        get => _eventDate;
        set => SetField(ref _eventDate, value);
    }

    public TimeSpan EventTime
    {
        get => _eventTime;
        set => SetField(ref _eventTime, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public AdminEditEventPage(int eventId)
    {
        InitializeComponent();
        _eventId = eventId;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (UserSession.Role != UserRole.Admin)
        {
            await DisplayAlert("Access Denied", "Only administrators can edit events.", "OK");
            await Navigation.PopAsync();
            return;
        }

        if (_isLoaded)
        {
            return;
        }

        await LoadEventAsync();
        _isLoaded = true;
    }

    private async Task LoadEventAsync()
    {
        try
        {
            _eventItem = await _databaseService.GetEventByIdAsync(_eventId);
            if (_eventItem is null)
            {
                await DisplayAlert("Error", "Event not found.", "OK");
                await Navigation.PopAsync();
                return;
            }

            TitleInput = _eventItem.Title;
            LocationInput = _eventItem.Location;
            OrganizerInput = _eventItem.Organizer;
            DescriptionInput = _eventItem.Description;
            EventDate = _eventItem.Date.Date;
            EventTime = _eventItem.Date.TimeOfDay;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load event: {ex.Message}", "OK");
            await Navigation.PopAsync();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_eventItem is null)
        {
            StatusMessage = "Event data was not loaded.";
            return;
        }

        if (string.IsNullOrWhiteSpace(TitleInput) ||
            string.IsNullOrWhiteSpace(LocationInput) ||
            string.IsNullOrWhiteSpace(OrganizerInput))
        {
            StatusMessage = "Title, location, and organizer are required.";
            return;
        }

        _eventItem.Title = TitleInput.Trim();
        _eventItem.Location = LocationInput.Trim();
        _eventItem.Organizer = OrganizerInput.Trim();
        _eventItem.Description = DescriptionInput.Trim();
        _eventItem.Date = EventDate.Date.Add(EventTime);

        try
        {
            var updated = await _databaseService.UpdateEventAsync(_eventItem);
            if (!updated)
            {
                StatusMessage = "No changes were saved.";
                return;
            }

            await DisplayAlert("Success", "Event updated successfully.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to save changes: {ex.Message}";
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
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
