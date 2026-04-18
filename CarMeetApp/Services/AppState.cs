using CarMeetApp.Models;

namespace CarMeetApp.Services;

public static class AppState
{
    private static readonly List<EventItem> _events =
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

    private static readonly Dictionary<string, List<EventItem>> _joinedEventsByUser = new(StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyList<EventItem> Events => _events.OrderBy(x => x.Date).ToList();

    public static void AddEvent(EventItem eventItem)
    {
        _events.Add(eventItem);
    }

    public static void JoinEvent(string email, string eventTitle)
    {
        var selectedEvent = _events.FirstOrDefault(x => x.Title.Equals(eventTitle, StringComparison.OrdinalIgnoreCase));
        if (selectedEvent is null)
        {
            return;
        }

        if (!_joinedEventsByUser.TryGetValue(email, out var joined))
        {
            joined = [];
            _joinedEventsByUser[email] = joined;
        }

        if (joined.All(x => !x.Title.Equals(selectedEvent.Title, StringComparison.OrdinalIgnoreCase)))
        {
            joined.Add(selectedEvent);
        }
    }

    public static List<EventItem> GetJoinedEvents(string email)
    {
        if (_joinedEventsByUser.TryGetValue(email, out var joined))
        {
            return joined.OrderBy(x => x.Date).ToList();
        }

        return [];
    }
}
