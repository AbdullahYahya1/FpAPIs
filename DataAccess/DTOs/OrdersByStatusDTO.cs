﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class OrdersByStatusDTO
    {
        public string Status { get; set; }
        public int TotalOrders { get; set; }
    }

}
