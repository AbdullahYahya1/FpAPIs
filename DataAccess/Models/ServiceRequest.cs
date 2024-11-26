using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

public enum ServiceRequestStatus
{
    New=0,
    InProgress=1,
    Resolved=2,
    Rejected=3
}

public enum RequestType
{
    Repair=0,
    Return=1,
    Sell=2,
    Dontate = 3
}

public class ServiceRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RequestId { get; set; }
    [Required]
    public string CreatedById { get; set; }
    public virtual AppUser CreatedBy { get; set; }
    public RequestType RequestType { get; set; }
    [StringLength(500)]
    public string ProductDetails { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal? RequestedPrice { get; set; }
    [Column(TypeName = "decimal(7, 2)")]
    public decimal? PurchasePrice { get; set; }
    public ServiceRequestStatus ServiceRequestStatus { get; set; }
    public DateTime SubmissionDate { get; set; }
    public DateTime? ResponseDate { get; set; }
    [StringLength(2000)]
    public string? ResponseDetails { get; set; }

    public virtual ICollection<ServiceImage> Images { get; set; } = new List<ServiceImage>();
}
