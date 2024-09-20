using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

public enum TransactionStatus
{
    Payed,
    Failed
}

public enum PaymentProvider
{
    Visa,
    MasterCard,
    Mada,
    Cash
}

public class UserPurchaseTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    [Required]
    public string CreatedById { get; set; }
    public virtual AppUser CreatedBy { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(10, 2)")] 
    public decimal TotalPrice { get; set; }

    public TransactionStatus TransactionStatus { get; set; }

    public PaymentProvider Provider { get; set; }

    [StringLength(64)]
    public string CardholderName { get; set; }

}
