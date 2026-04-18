using Microsoft.EntityFrameworkCore;
using CarMeetApp.Data;
using CarMeetApp.Models;

namespace CarMeetApp.Services;

public class DatabaseService
{
    private readonly CarMeetDbContext _context;

    public DatabaseService()
    {
        _context = new CarMeetDbContext();
        _context.Database.EnsureCreated();
    }

    public DatabaseService(CarMeetDbContext context)
    {
        _context = context;
    }

    // Event operations
    public async Task<List<EventItem>> GetEventsAsync()
    {
        return await _context.Events.OrderBy(e => e.Date).ToListAsync();
    }

    public async Task<EventItem?> GetEventByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.EventUsers)
            .ThenInclude(eu => eu.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<EventItem> AddEventAsync(EventItem eventItem)
    {
        eventItem.CreatedAt = DateTime.UtcNow;
        
        // Ensure Organizer field is set
        if (string.IsNullOrWhiteSpace(eventItem.Organizer))
        {
            eventItem.Organizer = "Admin Team"; // Default fallback
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
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem != null)
        {
            _context.Events.Remove(eventItem);
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }

    // User operations
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

    // Event sign-up operations
    public async Task<bool> SignUpForEventAsync(string userEmail, int eventId, string carBrand, string carModel, string carGeneration, int? horsepower, double? engineSize)
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

        // Check if user is already signed up
        var existingSignUp = await _context.EventUsers
            .FirstOrDefaultAsync(eu => eu.UserId == user.Id && eu.EventId == eventId);

        if (existingSignUp != null)
        {
            return false; // Already signed up
        }

        var eventUser = new EventUser
        {
            UserId = user.Id,
            EventId = eventId,
            CarBrand = carBrand,
            CarModel = carModel,
            CarGeneration = carGeneration,
            CarHorsepowerHp = horsepower,
            CarEngineSizeLiters = engineSize
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

    public void Dispose()
    {
        _context.Dispose();
    }
}
