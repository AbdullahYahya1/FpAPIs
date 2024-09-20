using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace DataAccess.DTOs
{
    public class PayOrderDto
    {
        [StringLength(16)]
        public string? CreditCardNumber { get; set; }

        [StringLength(64)]
        public string CardholderName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }
        [StringLength(3)]
        public string? CVV { get; set; }

        public bool Cash {  get; set; } 
    }
}
