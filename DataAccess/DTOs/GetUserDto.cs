using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.DTOs
{
    public class GetUserDto
    {
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public UserType UserType { get; set; }
        public string UserImageURL { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string MobileNumber { get; set; }
    }

}
