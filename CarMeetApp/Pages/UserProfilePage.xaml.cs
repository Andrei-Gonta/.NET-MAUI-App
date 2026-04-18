using CarMeetApp.Models;
using CarMeetApp.Services;

namespace CarMeetApp.Pages;

public partial class UserProfilePage : ContentPage
{
    public UserProfile Profile { get; } = new FakeDataService().GetProfile();

    public UserProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
