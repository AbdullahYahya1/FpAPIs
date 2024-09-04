using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Models
{
    public enum ProductStatus
    {
        Active,
        Inactive,
        OutOfStock,
        Discontinued
    }

    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        [StringLength(100)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(100)]
        public string NameEn { get; set; }

        [StringLength(500)]
        public string DescriptionAr { get; set; }

        [StringLength(500)]
        public string DescriptionEn { get; set; }

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

        public int StyleId { get; set; }
        public virtual Style Style { get; set; }

        [StringLength(50)]
        public string Color { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal Height { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal Width { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal Weight { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price { get; set; }

        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ProductStatus Status { get; set; }
    }

}