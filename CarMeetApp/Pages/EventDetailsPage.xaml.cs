using CarMeetApp.Models;
using CarMeetApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarMeetApp.Pages;

public partial class EventDetailsPage : ContentPage, INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService = new();
    private EventItem? _featuredEvent;

    public new event PropertyChangedEventHandler? PropertyChanged;

    public EventItem? FeaturedEvent
    {
        get => _featuredEvent;
        set => SetField(ref _featuredEvent, value);
    }

    public EventDetailsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var events = await _databaseService.GetEventsAsync();
            FeaturedEvent = events.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load event details: {ex.Message}", "OK");
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
