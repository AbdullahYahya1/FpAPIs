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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly FPDbContext _context;
        public OrderRepository(FPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync(string customerId)
        {
            return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Transaction)
            .Include(o => o.ShippingAddress)
            .Include(o => o.OrderItems)
                .ThenInclude(OI=>OI.Product)
                    .ThenInclude(P=>P.Brand)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Images)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Style)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Material)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Category)
            .Include(o => o.Transaction)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Transaction)
            .Include(o => o.ShippingAddress)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Brand)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Images)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Style)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Material)
            .Include(o => o.OrderItems)
                .ThenInclude(OI => OI.Product)
                    .ThenInclude(P => P.Category)
            .Include(o => o.Transaction)
            .ToListAsync();
        }
    }
}
