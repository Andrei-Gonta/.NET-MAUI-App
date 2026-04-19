using CarMeetApp.Models;
using CarMeetApp.Services;
using System.Collections.ObjectModel;

namespace CarMeetApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    public ObservableCollection<EventItem> UpcomingEvents { get; } = [];
    public ObservableCollection<GalleryItem> LatestEventPhotos { get; } =
    [
        new GalleryItem(
            "https://images.unsplash.com/photo-1493238792000-8113da705763?auto=format&fit=crop&w=1000&q=80",
            "Neon Night Showcase - Seattle"),
        new GalleryItem(
            "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=1000&q=80",
            "Sunset Street Meet - Austin"),
        new GalleryItem(
            "https://images.unsplash.com/photo-1542282088-72c9c27ed0cd?auto=format&fit=crop&w=1000&q=80",
            "Mountain Drive Meetup - Blue Ridge")
    ];

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

public record GalleryItem(string ImageUrl, string Caption);
