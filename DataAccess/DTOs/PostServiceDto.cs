using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DataAccess.DTOs
{
    public class PostServiceDto
    {
        public RequestType RequestType { get; set; }
        public string ProductDetails { get; set; }

        [Column(TypeName = "decimal(7, 2)")]
        public decimal? RequestedPrice { get; set; }
        [Column(TypeName = "decmail(7,2)")]
        public decimal? PurchasePrice { get; set; }

        public virtual ICollection<string> ImagesString64 { get; set; } = new List<string>();
    }
}
