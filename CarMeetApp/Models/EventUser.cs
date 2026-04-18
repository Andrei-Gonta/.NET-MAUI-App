using System.ComponentModel.DataAnnotations;

namespace CarMeetApp.Models;

public class EventUser
{
    public int EventId { get; set; }
    public virtual EventItem Event { get; set; } = null!;
    
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public DateTime SignedUpAt { get; set; } = DateTime.UtcNow;
    
    // Additional information about the car used for this event
    [StringLength(50)]
    public string CarBrand { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string CarModel { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string CarGeneration { get; set; } = string.Empty;
    
    public int? CarHorsepowerHp { get; set; }
    
    public double? CarEngineSizeLiters { get; set; }
}
