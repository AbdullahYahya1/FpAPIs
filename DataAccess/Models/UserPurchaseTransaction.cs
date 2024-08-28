using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed,
    Refunded,
    Cancelled
}

public enum PaymentProvider
{
    CreditCard,
    PayPal,
    BankTransfer,
    CashOnDelivery,
    StoreCredit,
    GiftCard
}

public class UserPurchaseTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    [Required]
    public string CreatedById { get; set; }
    public virtual AppUser CreatedBy { get; set; }

    public DateTime TransactionDate { get; set; }
    [Column(TypeName = "decimal(7, 2)")]
    public decimal TotalPrice { get; set; }

    public TransactionStatus Status { get; set; }

    public PaymentProvider Provider { get; set; }

    [StringLength(500)]
    public string AccountDetails { get; set; }

    [StringLength(1000)]
    public string Notes { get; set; }
}
