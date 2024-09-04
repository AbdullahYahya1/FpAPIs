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
    public class PostBrandDto
    {
        public string BrandName { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ReputationScore { get; set; }
        public int EstablishmentYear { get; set; }
        public string CountryOfOrigin { get; set; }
        public string ContactInfo { get; set; }
    }
}
