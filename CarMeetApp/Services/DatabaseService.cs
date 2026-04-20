using Microsoft.EntityFrameworkCore;
using CarMeetApp.Data;
using CarMeetApp.Models;
using System.Text.Json;

namespace CarMeetApp.Services;

public class DatabaseService
{
    private readonly CarMeetDbContext _context;

    public DatabaseService()
    {
        _context = new CarMeetDbContext();
        _context.Database.EnsureCreated();
        EnsureEventUserPhotoColumn();
        EnsureUserProfileColumns();
        RemovePredefinedEvents();
    }

    public DatabaseService(CarMeetDbContext context)
    {
        _context = context;
        EnsureEventUserPhotoColumn();
        EnsureUserProfileColumns();
        RemovePredefinedEvents();
    }

    public async Task<List<EventItem>> GetEventsAsync()
    {
        return await _context.Events
            .AsNoTracking()
            .OrderBy(e => e.Date)
            .ToListAsync();
    }

    public async Task<EventItem?> GetEventByIdAsync(int id)
    {
        return await _context.Events
            .AsNoTracking()
            .Include(e => e.EventUsers)
            .ThenInclude(eu => eu.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EventItem> AddEventAsync(EventItem eventItem)
    {
        eventItem.CreatedAt = DateTime.UtcNow;
        
        if (string.IsNullOrWhiteSpace(eventItem.Organizer))
        {
            eventItem.Organizer = "Admin Team";
        }
        
        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();
        return eventItem;
    }

    public async Task<bool> UpdateEventAsync(EventItem eventItem)
    {
        _context.Events.Update(eventItem);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE Users SET EventItemId = NULL WHERE EventItemId = {id}");
        }
        catch
        {
        }

        await _context.EventUsers
            .Where(eu => eu.EventId == id)
            .ExecuteDeleteAsync();

        var deletedEvents = await _context.Events
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();

        return deletedEvents > 0;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.EventUsers)
            .ThenInclude(eu => eu.Event)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> AddUserAsync(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> SignUpForEventAsync(
        string userEmail,
        int eventId,
        string carBrand,
        string carModel,
        string carGeneration,
        int? horsepower,
        double? engineSize,
        List<string> carPhotoPaths)
    {
        var user = await GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return false;
        }

        var eventItem = await GetEventByIdAsync(eventId);
        if (eventItem == null)
        {
            return false;
        }

        var existingSignUp = await _context.EventUsers
            .FirstOrDefaultAsync(eu => eu.UserId == user.Id && eu.EventId == eventId);

        if (existingSignUp != null)
        {
            return false;
        }

        var eventUser = new EventUser
        {
            UserId = user.Id,
            EventId = eventId,
            CarBrand = carBrand,
            CarModel = carModel,
            CarGeneration = carGeneration,
            CarHorsepowerHp = horsepower,
            CarEngineSizeLiters = engineSize,
            CarPhotoPathsJson = JsonSerializer.Serialize(carPhotoPaths ?? [])
        };

        _context.EventUsers.Add(eventUser);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<EventItem>> GetUserSignedUpEventsAsync(string userEmail)
    {
        var user = await GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return new List<EventItem>();
        }

        return await _context.EventUsers
            .Where(eu => eu.UserId == user.Id)
            .Select(eu => eu.Event)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }

    public async Task<List<User>> GetEventSignedUpUsersAsync(int eventId)
    {
        return await _context.EventUsers
            .Where(eu => eu.EventId == eventId)
            .Select(eu => eu.User)
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }

    public async Task<bool> CancelEventSignUpAsync(string userEmail, int eventId)
    {
        var user = await GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return false;
        }

        var eventUser = await _context.EventUsers
            .FirstOrDefaultAsync(eu => eu.UserId == user.Id && eu.EventId == eventId);

        if (eventUser != null)
        {
            _context.EventUsers.Remove(eventUser);
            return await _context.SaveChangesAsync() > 0;
        }

        return false;
    }

    public async Task<List<EventUser>> GetEventUserDetailsAsync(int eventId)
    {
        return await _context.EventUsers
            .Where(eu => eu.EventId == eventId)
            .Include(eu => eu.User)
            .OrderBy(eu => eu.User.FullName)
            .ToListAsync();
    }

    public async Task<List<string>> GetParticipantCarPhotosAsync(int eventId, int userId)
    {
        var eventUser = await _context.EventUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(eu => eu.EventId == eventId && eu.UserId == userId);

        if (eventUser is null || string.IsNullOrWhiteSpace(eventUser.CarPhotoPathsJson))
        {
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(eventUser.CarPhotoPathsJson) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private void EnsureEventUserPhotoColumn()
    {
        try
        {
            _context.Database.ExecuteSqlRaw("ALTER TABLE EventUsers ADD COLUMN CarPhotoPathsJson TEXT NOT NULL DEFAULT '[]';");
        }
        catch
        {
        }
    }

    private void EnsureUserProfileColumns()
    {
        TryAddUserColumn("ALTER TABLE Users ADD COLUMN Location TEXT NOT NULL DEFAULT 'N/A';");
        TryAddUserColumn("ALTER TABLE Users ADD COLUMN Age TEXT NOT NULL DEFAULT 'N/A';");
        TryAddUserColumn("ALTER TABLE Users ADD COLUMN SocialLinks TEXT NOT NULL DEFAULT 'N/A';");
        TryAddUserColumn("ALTER TABLE Users ADD COLUMN AvatarPhotoPath TEXT NOT NULL DEFAULT '';");
        TryAddUserColumn("ALTER TABLE Users ADD COLUMN ShortDescription TEXT NOT NULL DEFAULT 'N/A';");
        TryAddUserColumn("ALTER TABLE Users ADD COLUMN CarDescription TEXT NOT NULL DEFAULT 'N/A';");
    }

    private void TryAddUserColumn(string sql)
    {
        try
        {
            _context.Database.ExecuteSqlRaw(sql);
        }
        catch
        {
        }
    }

    private void RemovePredefinedEvents()
    {
        try
        {
            _context.Database.ExecuteSqlRaw("""
                UPDATE Users
                SET EventItemId = NULL
                WHERE EventItemId IN (
                    SELECT Id FROM Events
                    WHERE Title IN ('Sunset Street Meet', 'Mountain Drive Meetup', 'Neon Night Showcase')
                );
                """);

            _context.Database.ExecuteSqlRaw("""
                DELETE FROM EventUsers
                WHERE EventId IN (
                    SELECT Id FROM Events
                    WHERE Title IN ('Sunset Street Meet', 'Mountain Drive Meetup', 'Neon Night Showcase')
                );
                """);

            _context.Database.ExecuteSqlRaw("""
                DELETE FROM Events
                WHERE Title IN ('Sunset Street Meet', 'Mountain Drive Meetup', 'Neon Night Showcase');
                """);
        }
        catch
        {
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
