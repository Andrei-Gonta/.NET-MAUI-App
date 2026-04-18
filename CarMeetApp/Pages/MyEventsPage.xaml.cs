using CarMeetApp.Models;
using CarMeetApp.Services;
using System.Collections.ObjectModel;

namespace CarMeetApp.Pages;

public partial class MyEventsPage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    public ObservableCollection<EventItem> JoinedEvents { get; } = [];

    public MyEventsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        JoinedEvents.Clear();

        if (!UserSession.IsLoggedIn)
        {
            return;
        }

        try
        {
            var joinedEvents = await _databaseService.GetUserSignedUpEventsAsync(UserSession.Email);
            foreach (var eventItem in joinedEvents)
            {
                JoinedEvents.Add(eventItem);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load your events: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
