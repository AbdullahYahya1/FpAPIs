using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }

        [Required]
        public int ProductId { get; set; } 

        public virtual Product Product { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }  
    }
}
