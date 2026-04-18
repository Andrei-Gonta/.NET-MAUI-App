using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class AdminAddEventPage : ContentPage, INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService = new();
    private string _titleInput = string.Empty;
    private string _locationInput = string.Empty;
    private string _organizerInput = "Admin Team";
    private string _descriptionInput = string.Empty;
    private DateTime _eventDate = DateTime.Today.AddDays(1);
    private TimeSpan _eventTime = new(18, 0, 0);
    private string _statusMessage = string.Empty;

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

    public AdminAddEventPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnAddEventClicked(object sender, EventArgs e)
    {
        if (UserSession.Role != UserRole.Admin)
        {
            StatusMessage = "Only admins can add events.";
            return;
        }

        if (string.IsNullOrWhiteSpace(TitleInput) ||
            string.IsNullOrWhiteSpace(LocationInput) ||
            string.IsNullOrWhiteSpace(OrganizerInput))
        {
            StatusMessage = "Please provide title, location, and organizer.";
            return;
        }

        try
        {
            var newEvent = new EventItem
            {
                Title = TitleInput,
                Location = LocationInput,
                Organizer = OrganizerInput,
                Description = string.IsNullOrWhiteSpace(DescriptionInput) ? "New event added by admin." : DescriptionInput,
                Date = EventDate.Date.Add(EventTime)
            };

            await _databaseService.AddEventAsync(newEvent);
            StatusMessage = $"Event '{TitleInput}' added successfully.";

            TitleInput = string.Empty;
            LocationInput = string.Empty;
            DescriptionInput = string.Empty;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error adding event: {ex.Message}";
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
