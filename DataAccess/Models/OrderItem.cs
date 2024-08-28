using DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class OrderItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderItemId { get; set; }

    [Required]
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    [Required]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    public int Quantity { get; set; }
}
