using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class GetProductDto
    {
        public int ProductId { get; set; }
        public GetCategoryDto Category { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public GetMaterialDto Material { get; set; }
        public GetStyleDto Style { get; set; }
        public Color Color { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public GetBrandDto Brand { get; set; }
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
        public ProductStatus ProductStatus { get; set; }
    }
}
