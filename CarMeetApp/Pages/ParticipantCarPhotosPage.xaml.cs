using System.Collections.ObjectModel;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class ParticipantCarPhotosPage : ContentPage
{
    private readonly DatabaseService _databaseService = new();
    private readonly int _eventId;
    private readonly int _userId;
    private readonly string _participantName;

    public ObservableCollection<string> PhotoPaths { get; } = [];

    public string HeaderText => $"Car pictures: {_participantName}";

    public bool HasNoPhotos => PhotoPaths.Count == 0;

    public ParticipantCarPhotosPage(int eventId, int userId, string participantName)
    {
        InitializeComponent();
        _eventId = eventId;
        _userId = userId;
        _participantName = participantName;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPhotosAsync();
    }

    private async Task LoadPhotosAsync()
    {
        try
        {
            var photoPaths = await _databaseService.GetParticipantCarPhotosAsync(_eventId, _userId);
            PhotoPaths.Clear();

            foreach (var path in photoPaths.Where(File.Exists))
            {
                PhotoPaths.Add(path);
            }

            OnPropertyChanged(nameof(HasNoPhotos));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load car pictures: {ex.Message}", "OK");
        }
    }
}
