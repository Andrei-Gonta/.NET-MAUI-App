using System.Runtime.CompilerServices;
using CarMeetApp.Models;
using CarMeetApp.Services;
using System.Collections.ObjectModel;

namespace CarMeetApp.Pages;

public partial class UniversalEventDetailsPage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    private int _eventId;
    private EventItem? _event;

    public string EventTitle => _event?.Title ?? string.Empty;
    public string EventLocation => _event?.Location ?? string.Empty;
    public string EventDate => _event?.Date.ToString("dddd, MMMM d, yyyy 'at' h:mm tt") ?? string.Empty;
    public string EventOrganizer => $"Organized by {Normalize(_event?.Organizer, "Admin Team")}";
    public string EventDescription => _event?.Description ?? string.Empty;
    public string ParticipantsCount => $"{Participants.Count} Participant{Participants.Count.Pluralize()}";
    public bool HasNoParticipants => Participants.Count == 0;

    public ObservableCollection<ParticipantViewModel> Participants { get; } = [];

    public UniversalEventDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public UniversalEventDetailsPage(int eventId) : this()
    {
        _eventId = eventId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_eventId <= 0)
        {
            return;
        }

        await LoadEventDetailsAsync(_eventId);
    }

    private async Task LoadEventDetailsAsync(int eventId)
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
            var mappedParticipants = eventUsers.Select(eu => new ParticipantViewModel
            {
                FullName = eu.User.FullName,
                Email = eu.User.Email,
                PhoneNumber = eu.User.PhoneNumber,
                CarInfo = $"{eu.CarBrand} {eu.CarModel} ({eu.CarGeneration})",
                CarSpecs = GetCarSpecs(eu.CarHorsepowerHp, eu.CarEngineSizeLiters),
                SignUpDate = $"Signed up: {eu.SignedUpAt:MMMM d, yyyy 'at' h:mm tt}"
            }).ToList();

            Participants.Clear();
            foreach (var participant in mappedParticipants)
            {
                Participants.Add(participant);
            }

            OnPropertyChanged(nameof(ParticipantsCount));
            OnPropertyChanged(nameof(HasNoParticipants));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load participants: {ex.Message}", "OK");
        }
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

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private static string Normalize(string? value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value;

}
