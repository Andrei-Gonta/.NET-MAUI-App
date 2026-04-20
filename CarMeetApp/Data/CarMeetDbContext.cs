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
            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "CarMeetApp.db");
                optionsBuilder.UseSqlite($"Data Source={databasePath}");
            }
            catch
            {
                optionsBuilder.UseSqlite("Data Source=CarMeetApp.db");
            }

            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}
