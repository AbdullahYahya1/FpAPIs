using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum UserType
{
    Client = 0,
    Manager = 1,
    Support = 2,
    DeliveryRepresentative = 3 // مندوب توصيل
}

public class AppUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string UserId { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string UserName { get; set; }

    [StringLength(100)]
    public string FullName { get; set; }

    [StringLength(500)]
    public string UserImageURL { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public virtual AppUser? CreatedBy { get; set; }
    public string? CreatedById { get; set; }

    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

    public virtual AppUser? UpdateBy { get; set; }
    public string? UpdateById { get; set; }

    public UserType UserType { get; set; }

    [Phone]
    public string MobileNumber { get; set; }

    [Required]
    public string Password { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
    public virtual ICollection<UserAddress> Addresses { get; set; }
}
