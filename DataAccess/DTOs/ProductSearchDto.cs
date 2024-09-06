using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ProductSearchDto
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Color? Color { get; set; }
        public int? BrandId { get; set; }
        public int? MaterialId { get; set; }
        public int? StyleId { get; set; }
        public ProductStatus? ProductStatus { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; } = "Name";
        public bool IsDescending { get; set; } = false;
    }

}
