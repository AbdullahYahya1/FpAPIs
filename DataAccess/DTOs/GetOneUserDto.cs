using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Models;

namespace DataAccess.DTOs
{
    public class GetOneUserDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string MobileNumber { get; set; }
    }
}
