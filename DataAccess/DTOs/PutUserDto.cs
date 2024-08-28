﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class PutUserDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Base64Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
    }
}
