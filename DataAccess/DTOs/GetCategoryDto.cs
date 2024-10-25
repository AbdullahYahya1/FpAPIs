using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class GetCategoryDto
    {
        public int CategoryId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
