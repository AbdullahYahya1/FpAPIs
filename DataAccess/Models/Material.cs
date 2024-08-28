using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Material
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialId { get; set; }

        [Required]
        [StringLength(100)]
        public string MaterialNameAr { get; set; }

        [Required]
        [StringLength(100)]
        public string MaterialNameEn { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

}