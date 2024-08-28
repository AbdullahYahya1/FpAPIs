using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class AuthenticateModel
    {
        public string EmailORUserName { get; set; }
        public string Password { get; set; }
    }
}
