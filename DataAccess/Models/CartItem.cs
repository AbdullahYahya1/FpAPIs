using DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartItemId { get; set; }

    [Required]
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }

    [Required]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}
