using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models;

public enum ServiceRequestStatus
{
    New,
    InProgress,
    Resolved,
    Closed,
    Rejected
}

public enum RequestType
{
    Repair,
    Return,
    Sell,
    buy
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

    public ServiceRequestStatus Status { get; set; }

    public DateTime SubmissionDate { get; set; }

    public DateTime? ResponseDate { get; set; }


    [StringLength(2000)]
    public string ResponseDetails { get; set; }
    public virtual ICollection<ServiceImage> Images { get; set; } = new List<ServiceImage>();
}
