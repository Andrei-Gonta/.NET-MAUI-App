using System.ComponentModel.DataAnnotations;

namespace CarMeetApp.Models;

public class EventItem
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string Location { get; set; } = string.Empty;
    
    public DateTime Date { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Organizer { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
    
    // Helper property to get users who signed up
    public virtual ICollection<User> SignedUpUsers => EventUsers.Select(eu => eu.User).ToList();
}
