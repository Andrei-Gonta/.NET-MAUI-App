using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;
using System.Collections.ObjectModel;

namespace CarMeetApp.Pages;

public partial class AdminEventDetailsPage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    private int _eventId;
    private EventItem? _event;
    private List<ParticipantViewModel> _participants = [];

    public string EventTitle => _event?.Title ?? string.Empty;
    public string EventLocation => _event?.Location ?? string.Empty;
    public string EventDate => _event?.Date.ToString("dddd, MMMM d, yyyy 'at' h:mm tt") ?? string.Empty;
    public string EventOrganizer => $"Organized by {Normalize(_event?.Organizer, "Admin Team")}";
    public string EventDescription => _event?.Description ?? string.Empty;
    public string ParticipantsCount => $"{_participants.Count} Participant{_participants.Count.Pluralize()}";
    public bool HasNoParticipants => _participants.Count == 0;

    public ObservableCollection<ParticipantViewModel> Participants { get; } = [];

    public AdminEventDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public AdminEventDetailsPage(int eventId) : this()
    {
        _eventId = eventId;
        LoadEventDetails(eventId);
    }

    private async void LoadEventDetails(int eventId)
    {
        try
        {
            _event = await _databaseService.GetEventByIdAsync(eventId);
            if (_event == null)
            {
                await DisplayAlert("Error", "Event not found.", "OK");
                await Navigation.PopAsync();
                return;
            }

            await LoadParticipants();
            OnPropertyChanged(nameof(EventTitle));
            OnPropertyChanged(nameof(EventLocation));
            OnPropertyChanged(nameof(EventDate));
            OnPropertyChanged(nameof(EventOrganizer));
            OnPropertyChanged(nameof(EventDescription));
            OnPropertyChanged(nameof(ParticipantsCount));
            OnPropertyChanged(nameof(HasNoParticipants));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load event details: {ex.Message}", "OK");
        }
    }

    private async Task LoadParticipants()
    {
        try
        {
            var eventUsers = await _databaseService.GetEventUserDetailsAsync(_event!.Id);
            _participants = eventUsers.Select(eu => new ParticipantViewModel
            {
                EventId = eu.EventId,
                UserId = eu.UserId,
                FullName = eu.User.FullName,
                Email = eu.User.Email,
                PhoneNumber = eu.User.PhoneNumber,
                CarInfo = $"{eu.CarBrand} {eu.CarModel} ({eu.CarGeneration})",
                CarSpecs = GetCarSpecs(eu.CarHorsepowerHp, eu.CarEngineSizeLiters),
                SignUpDate = $"Signed up: {eu.SignedUpAt:MMMM d, yyyy 'at' h:mm tt}"
            }).ToList();

            Participants.Clear();
            foreach (var participant in _participants)
            {
                Participants.Add(participant);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load participants: {ex.Message}", "OK");
        }
    }

    private async void OnSeeCarClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not ParticipantViewModel participant)
        {
            return;
        }

        await Navigation.PushAsync(new ParticipantCarPhotosPage(_eventId, participant.UserId, participant.FullName));
    }

    private string GetCarSpecs(int? horsepower, double? engineSize)
    {
        var specs = new List<string>();
        
        if (horsepower.HasValue)
            specs.Add($"{horsepower.Value} HP");
        
        if (engineSize.HasValue)
            specs.Add($"{engineSize.Value:F1}L");
        
        return specs.Count > 0 ? string.Join(" | ", specs) : "No specs available";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (UserSession.Role != UserRole.Admin)
        {
            DisplayAlert("Access Denied", "Only administrators can view event participants.", "OK").Wait();
            Navigation.PopAsync().Wait();
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }

    private static string Normalize(string? value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value;
}

public class ParticipantViewModel
{
    public int EventId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CarInfo { get; set; } = string.Empty;
    public string CarSpecs { get; set; } = string.Empty;
    public string SignUpDate { get; set; } = string.Empty;
}

public static class StringExtensions
{
    public static string Pluralize(this int count)
    {
        return count == 1 ? "" : "s";
    }
}
