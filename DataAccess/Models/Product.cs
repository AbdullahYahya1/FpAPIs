using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Models
{
    public enum ProductStatus
    {
        Active=0,
        Inactive=1,
    }
    public enum Color
    {
        Black = 0,
        White = 1,
        Red = 2,
        Green = 3,
        Blue = 4,
        Yellow = 5,
        Orange = 6,
        Purple = 7,
        Pink = 8,
        Gray = 9,
        Brown = 10,
        Cyan = 11,
        Magenta = 12
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

        public Color Color { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Height { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Width { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Weight { get; set; }

        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ProductStatus ProductStatus { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    }
}