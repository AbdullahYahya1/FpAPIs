using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class UserAddress
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AddressId { get; set; }

    [Required]
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }

    [StringLength(255)]
    public string StreetAddress { get; set; }

    [StringLength(100)]
    public string City { get; set; }

    [StringLength(100)]
    public string State { get; set; }

    [StringLength(50)]
    public string ZipCode { get; set; }

    [StringLength(100)]
    public string Country { get; set; }
}
