using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Common;


public class AppUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string UserId { get; set; }

    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? UserName { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public UserType UserType { get; set; }

    [Phone]
    public string MobileNumber { get; set; }
    public string? Password { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
    public virtual ICollection<UserAddress> Addresses { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<WishlistItem>  WishlistItems { get; set; } = new List<WishlistItem>();

}
