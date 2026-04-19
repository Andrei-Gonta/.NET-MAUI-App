using CarMeetApp.Models;
using CarMeetApp.Pages;
using CarMeetApp.Services;

namespace CarMeetApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BuildRoleTabs();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        UserSession.Logout();
        Application.Current.MainPage = new NavigationPage(new LoginPage());

        BuildRoleTabs();
    }

    private void BuildRoleTabs()
    {
        var tabBar = new TabBar();

        tabBar.Items.Add(CreateTab("Home", typeof(HomePage)));
        tabBar.Items.Add(CreateTab("Events", typeof(EventsPage)));

        if (UserSession.Role == UserRole.RegularUser)
        {
            tabBar.Items.Add(CreateTab("Sign Up", typeof(EventSignUpPage)));
            tabBar.Items.Add(CreateTab("My Events", typeof(MyEventsPage)));
        }

        if (UserSession.Role == UserRole.Admin)
        {
            tabBar.Items.Add(CreateTab("Admin Events", typeof(AdminEventsPage)));
            tabBar.Items.Add(CreateTab("Admin Add Event", typeof(AdminAddEventPage)));
        }

        tabBar.Items.Add(CreateTab("Profile", typeof(UserProfilePage)));
        Items.Add(tabBar);
        CurrentItem = tabBar.Items.FirstOrDefault();
    }

    private static Tab CreateTab(string title, Type pageType)
    {
        var tab = new Tab { Title = title };
        tab.Items.Add(new ShellContent
        {
            Title = title,
            ContentTemplate = new DataTemplate(pageType)
        });
        return tab;
    }
}
