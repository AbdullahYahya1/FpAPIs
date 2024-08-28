using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Style
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StyleId { get; set; }

        [Required]
        [StringLength(100)]
        public string StyleNameAr { get; set; }

        [Required]
        [StringLength(100)]
        public string StyleNameEn { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

}