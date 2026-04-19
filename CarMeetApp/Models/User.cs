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
    public string FullName { get; set; } = "empty";
    
    [StringLength(20)]
    public string PhoneNumber { get; set; } = "empty";

    [StringLength(100)]
    public string Location { get; set; } = "N/A";

    [StringLength(20)]
    public string Age { get; set; } = "N/A";

    [StringLength(500)]
    public string SocialLinks { get; set; } = "N/A";

    [StringLength(500)]
    public string AvatarPhotoPath { get; set; } = string.Empty;

    [StringLength(500)]
    public string ShortDescription { get; set; } = "N/A";

    [StringLength(200)]
    public string CarDescription { get; set; } = "N/A";
    
    [StringLength(50)]
    public string SelectedBrand { get; set; } = "N/A";
    
    [StringLength(50)]
    public string SelectedModel { get; set; } = "N/A";
    
    [StringLength(100)]
    public string SelectedGeneration { get; set; } = "N/A";
    
    public int? HorsepowerHp { get; set; }
    
    public double? EngineSizeLiters { get; set; }
    
    public UserRole Role { get; set; } = UserRole.RegularUser;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
}
