using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }

        [Required]
        [StringLength(100)]
        public string BrandName { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ReputationScore { get; set; }

        public int EstablishmentYear { get; set; }

        [StringLength(100)]
        public string CountryOfOrigin { get; set; }

        [StringLength(200)]
        public string ContactInfo { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

}