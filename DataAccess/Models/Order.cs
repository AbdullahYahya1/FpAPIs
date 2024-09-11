using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Returned
}

public enum ShippingStatus
{
    NotShipped,
    InTransit,
    OutForDelivery,
    Delivered,
    FailedDelivery
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

    public DateTime? ShippingDate { get; set; }
    [Column(TypeName = "decimal(7, 2)")]
    public decimal TotalPrice { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
