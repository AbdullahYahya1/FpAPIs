using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum OrderStatus
{
    Processing=0,
    Complete=1,
    Cancelled=2,
    Returned=3
}

public enum ShippingStatus
{
    NotShipped=0,
    InTransit=1,
    OutForDelivery=2,
    Delivered=3,
    FailedDelivery=4
}

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    [Required]
    public string CustomerId { get; set; }
    public virtual AppUser Customer { get; set; }

    public int? TransactionId { get; set; }
    public virtual UserPurchaseTransaction? Transaction { get; set; }

    public DateTime CreatedAt { get; set; }

    public OrderStatus Status { get; set; }

    public int ShippingAddressId { get; set; }
    public virtual UserAddress ShippingAddress { get; set; }

    public ShippingStatus ShippingStatus { get; set; }

    public string? DriverId { get; set; }
    public DateTime? ShippingDate { get; set; }
    [Column(TypeName = "decimal(7, 2)")]
    public decimal TotalPrice { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
