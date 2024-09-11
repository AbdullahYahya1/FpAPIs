using DataAccess.Context;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly FPDbContext _context;
        public CartItemRepository(FPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartItemProductsByUserID(string userId)
        {
            var items = await _context.CartItems
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category) 
                .Include(c => c.Product)
                    .ThenInclude(p => p.Brand)  
                .Include(c => c.Product)
                    .ThenInclude(p => p.Material)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Style) 
                .Include(c => c.Product)
                    .ThenInclude(p => p.Images) 
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return items;
        }
        public async Task RemoveCartItemsByProductIds(List<int> productIds)
        {
            var cartItems = _context.CartItems.Where(c => productIds.Contains(c.ProductId));
            _context.CartItems.RemoveRange(cartItems);
        }

        public async Task RemoveCartItemsForUserByProductIds(string userId, List<int> productIds)
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId && productIds.Contains(c.ProductId));
            _context.CartItems.RemoveRange(cartItems);
        }
    }
}
