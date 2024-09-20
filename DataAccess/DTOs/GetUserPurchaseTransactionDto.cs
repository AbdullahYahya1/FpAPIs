using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class GetUserPurchaseTransactionDto
    {
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        public PaymentProvider Provider { get; set; }
        public string CardholderName { get; set; }
    }
}
