using Microsoft.EntityFrameworkCore;
using CarMeetApp.Models;

namespace CarMeetApp.Data;

public class CarMeetDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<EventItem> Events { get; set; }
    public DbSet<EventUser> EventUsers { get; set; }

    public CarMeetDbContext()
    {
    }

    public CarMeetDbContext(DbContextOptions<CarMeetDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Use SQLite for development
            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "CarMeetApp.db");
                optionsBuilder.UseSqlite($"Data Source={databasePath}");
            }
            catch
            {
                // Fallback for design-time (when running migrations)
                optionsBuilder.UseSqlite("Data Source=CarMeetApp.db");
            }
            
            // Suppress pending model changes warning for database update
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure EventUser many-to-many relationship
        modelBuilder.Entity<EventUser>()
            .HasKey(eu => new { eu.EventId, eu.UserId });

        modelBuilder.Entity<EventUser>()
            .HasOne(eu => eu.Event)
            .WithMany(e => e.EventUsers)
            .HasForeignKey(eu => eu.EventId);

        modelBuilder.Entity<EventUser>()
            .HasOne(eu => eu.User)
            .WithMany(u => u.EventUsers)
            .HasForeignKey(eu => eu.UserId);

        // Seed initial events
        modelBuilder.Entity<EventItem>().HasData(
            new EventItem
            {
                Id = 1,
                Title = "Sunset Street Meet",
                Location = "Downtown Garage, Austin",
                Date = DateTime.Today.AddDays(3).AddHours(18),
                Organizer = "Turbo Club",
                Description = "Casual evening meet for all builds: imports, muscle, and classics.",
                CreatedAt = DateTime.UtcNow
            },
            new EventItem
            {
                Id = 2,
                Title = "Mountain Drive Meetup",
                Location = "Blue Ridge Scenic Point",
                Date = DateTime.Today.AddDays(7).AddHours(9),
                Organizer = "Apex Riders",
                Description = "Morning cruise with photo stops and coffee at the summit.",
                CreatedAt = DateTime.UtcNow
            },
            new EventItem
            {
                Id = 3,
                Title = "Neon Night Showcase",
                Location = "Riverside Plaza, Seattle",
                Date = DateTime.Today.AddDays(14).AddHours(20),
                Organizer = "NightShift Garage",
                Description = "Night showcase featuring custom lighting and audio builds.",
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
