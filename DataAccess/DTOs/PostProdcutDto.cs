using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class PostProdcutDto
    {

        public int CategoryId { get; set; }
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string DescriptionAr { get; set; }

        public string DescriptionEn { get; set; }

        public int MaterialId { get; set; }
        public int StyleId { get; set; }
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
        public virtual ICollection<string> ImagesString64 { get; set; } = new List<string>();
        public ProductStatus Status { get; set; }
    }
}
