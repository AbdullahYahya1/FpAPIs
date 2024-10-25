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
    public class GetServiceDto
    {
        public int RequestId { get; set; }
        public string UserPhone { get; set; }
        public RequestType RequestType { get; set; }
        public string ProductDetails { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? RequestedPrice { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? PurchasePrice { get; set; }
        public ServiceRequestStatus ServiceRequestStatus { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? ResponseDetails { get; set; }
        public virtual ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();
    }
}
