using CarMeetApp.Models;
using CarMeetApp.Services;
using System.Collections.ObjectModel;

namespace CarMeetApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    public ObservableCollection<EventItem> UpcomingEvents { get; } = [];

    public HomePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            UpcomingEvents.Clear();
            var events = await _databaseService.GetEventsAsync();
            foreach (var eventItem in events)
            {
                UpcomingEvents.Add(eventItem);
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
}
