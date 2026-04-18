using CarMeetApp.Models;
using CarMeetApp.Services;
using System.Collections.ObjectModel;

namespace CarMeetApp.Pages;

public partial class AdminEventsPage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    public ObservableCollection<EventItem> Events { get; } = [];

    public AdminEventsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Check if user is admin
        if (UserSession.Role != UserRole.Admin)
        {
            await DisplayAlert("Access Denied", "Only administrators can access this page.", "OK");
            await Navigation.PopAsync();
            return;
        }

        await LoadEvents();
    }

    private async Task LoadEvents()
    {
        try
        {
            Events.Clear();
            var events = await _databaseService.GetEventsAsync();
            foreach (var eventItem in events)
            {
                Events.Add(eventItem);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load events: {ex.Message}", "OK");
        }
    }

    private async void OnViewParticipantsClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int eventId)
        {
            await Navigation.PushAsync(new AdminEventDetailsPage(eventId));
        }
    }
}
