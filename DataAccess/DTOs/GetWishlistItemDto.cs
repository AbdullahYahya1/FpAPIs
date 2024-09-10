using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class GetWishlistItemDto
    {
        public virtual GetProductDto Product { get; set; }
    }
}
