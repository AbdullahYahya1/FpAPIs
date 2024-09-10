using DataAccess.Context;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class WishlistItemRepository : Repository<WishlistItem>, IWishlistItemRepository
    {
        public WishlistItemRepository(FPDbContext context) : base(context)
        {
        }
    }
}
