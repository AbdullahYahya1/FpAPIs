using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class TopCustomersDTO
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int TotalOrders { get; set; }
    }

}
