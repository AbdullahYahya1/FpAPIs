using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class UpdateRequestDto
    {
        public string? ResponseDetails { get; set; }
        public ServiceRequestStatus ServiceRequestStatus { get; set; }
    }
}
