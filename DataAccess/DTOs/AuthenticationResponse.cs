using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class AuthenticationResponse
    {
        public TokenResponse Tokens { get; set; }
        public GetOneUserDto User { get; set; }
    }
}
