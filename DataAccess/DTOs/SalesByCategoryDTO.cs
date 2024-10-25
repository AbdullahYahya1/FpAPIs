using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class SalesByCategoryDTO
    {
        public string Category { get; set; }
        public decimal TotalSales { get; set; }
    }

}
