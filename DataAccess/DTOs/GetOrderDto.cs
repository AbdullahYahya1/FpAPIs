using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class GetOrderDto
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public int ShippingAddressId { get; set; }
        public virtual GetAddressDto ShippingAddress { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
        public DateTime? ShippingDate { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal TotalPrice { get; set; }
        public virtual ICollection<GetOrderItemDto> OrderItems { get; set; }
    }
}
