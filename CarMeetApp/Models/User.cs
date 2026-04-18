using System.ComponentModel.DataAnnotations;

namespace CarMeetApp.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string SelectedBrand { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string SelectedModel { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string SelectedGeneration { get; set; } = string.Empty;
    
    public int? HorsepowerHp { get; set; }
    
    public double? EngineSizeLiters { get; set; }
    
    public UserRole Role { get; set; } = UserRole.RegularUser;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
}
