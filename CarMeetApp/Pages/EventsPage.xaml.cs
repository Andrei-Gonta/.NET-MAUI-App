using System.Collections.ObjectModel;
using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class EventsPage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    public ObservableCollection<EventItem> AvailableEvents { get; } = [];

    public EventsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadEventsAsync();
    }

    private async Task LoadEventsAsync()
    {
        try
        {
            AvailableEvents.Clear();
            var events = await _databaseService.GetEventsAsync();
            foreach (var eventItem in events)
            {
                AvailableEvents.Add(eventItem);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load events: {ex.Message}", "OK");
        }
    }

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int eventId)
        {
            await Navigation.PushAsync(new UniversalEventDetailsPage(eventId));
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }
}
