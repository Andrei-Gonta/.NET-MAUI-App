using CarMeetApp.Models;

namespace CarMeetApp.Services;

public class FakeDataService
{
    public List<EventItem> GetUpcomingEvents()
    {
        return
        [
            new EventItem
            {
                Title = "Sunset Street Meet",
                Location = "Downtown Garage, Austin",
                Date = DateTime.Today.AddDays(3).AddHours(18),
                Organizer = "Turbo Club",
                Description = "Casual evening meet for all builds: imports, muscle, and classics."
            },
            new EventItem
            {
                Title = "Mountain Drive Meetup",
                Location = "Blue Ridge Scenic Point",
                Date = DateTime.Today.AddDays(7).AddHours(9),
                Organizer = "Apex Riders",
                Description = "Morning cruise with photo stops and coffee at the summit."
            },
            new EventItem
            {
                Title = "Neon Night Showcase",
                Location = "Riverside Plaza, Seattle",
                Date = DateTime.Today.AddDays(14).AddHours(20),
                Organizer = "NightShift Garage",
                Description = "Night showcase featuring custom lighting and audio builds."
            }
        ];
    }

    public List<EventItem> GetMyEvents()
    {
        return
        [
            new EventItem
            {
                Title = "Retro Wheels Sunday",
                Location = "Maple Town Square",
                Date = DateTime.Today.AddDays(1).AddHours(10),
                Organizer = "City Classics",
                Description = "Community classic car meet with swap booths."
            },
            new EventItem
            {
                Title = "Track Prep Workshop",
                Location = "Northside Motorsport Center",
                Date = DateTime.Today.AddDays(9).AddHours(15),
                Organizer = "Grip Society",
                Description = "Hands-on session covering tires, alignment, and safety checks."
            }
        ];
    }

    public UserProfile GetProfile()
    {
        return new UserProfile
        {
            FullName = "Jordan Miller",
            Username = "@jordan_speed",
            FavoriteCar = "Nissan GT-R R35",
            Bio = "Weekend cruiser, occasional track day driver, and photo enthusiast.",
            MeetCount = 27
        };
    }
}
